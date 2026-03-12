#!/usr/bin/env node
/**
 * npm-compat-mcp — Servidor MCP para verificação de pacotes NPM
 * compatíveis com Next.js 15 e React 19
 *
 * Ferramentas disponíveis:
 *   - search_npm_package: busca pacotes no registry NPM
 *   - get_npm_versions: lista versões de um pacote específico
 *   - check_npm_compat: verifica compatibilidade com Next.js 15 / React 19
 */

import { McpServer } from "@modelcontextprotocol/sdk/server/mcp.js";
import { StdioServerTransport } from "@modelcontextprotocol/sdk/server/stdio.js";
import { z } from "zod";

const NPM_REGISTRY = "https://registry.npmjs.org";
const NPM_SEARCH = "https://registry.npmjs.org/-/v1/search";

const PEER_CONSTRAINTS = {
  next: "^15.0.0",
  react: "^19.0.0",
  "react-dom": "^19.0.0",
};

// ── Helpers ────────────────────────────────────────────────────────────────

async function fetchJson(url) {
  const res = await fetch(url, {
    headers: { "User-Agent": "npm-compat-mcp/1.0 (github.com/copilot-agent)" },
    signal: AbortSignal.timeout(10000),
  });
  if (!res.ok) throw new Error(`HTTP ${res.status}: ${url}`);
  return res.json();
}

/** Analisa peer dependencies e verifica conflitos com Next.js 15 / React 19 */
function analyzePeerDeps(peerDependencies = {}) {
  const issues = [];
  const ok = [];

  for (const [pkg, constraint] of Object.entries(peerDependencies)) {
    const ref = PEER_CONSTRAINTS[pkg];
    if (!ref) continue;

    // Verificação simples: se o constraint exige ^16, ^17 ou ^18 apenas, pode ser incompatível
    const requiresOldReact =
      pkg === "react" &&
      (constraint.includes("^16") || constraint.includes("^17") || constraint.includes("^18")) &&
      !constraint.includes("19") &&
      !constraint.includes("*") &&
      !constraint.includes(">=");

    const requiresOldNext =
      pkg === "next" &&
      !constraint.includes("15") &&
      !constraint.includes("*") &&
      !constraint.includes(">=");

    if (requiresOldReact || requiresOldNext) {
      issues.push(`\`${pkg}\` requer \`${constraint}\` (incompatível com ${ref})`);
    } else {
      ok.push(`\`${pkg}\`: \`${constraint}\` ✅`);
    }
  }

  return { issues, ok };
}

// ── Servidor MCP ────────────────────────────────────────────────────────────

const server = new McpServer({
  name: "npm-compat-mcp",
  version: "1.0.0",
});

// ── Ferramenta 1: search_npm_package ───────────────────────────────────────

server.tool(
  "search_npm_package",
  "Busca pacotes NPM por termo. Útil para encontrar bibliotecas para projetos Next.js 15 / React 19.",
  {
    query: z.string().describe("Termo de busca (ex: 'form validation react', 'shadcn', 'zustand state')"),
    size: z.number().min(1).max(20).default(10).describe("Número de resultados (1-20, padrão 10)"),
  },
  async ({ query, size }) => {
    const url = `${NPM_SEARCH}?text=${encodeURIComponent(query)}&size=${size}`;
    const data = await fetchJson(url);

    if (!data.objects?.length) {
      return { content: [{ type: "text", text: `Nenhum pacote encontrado para "${query}".` }] };
    }

    const lines = data.objects.map(({ package: pkg, score }) => {
      const quality = Math.round((score?.detail?.quality ?? 0) * 100);
      const maintenance = Math.round((score?.detail?.maintenance ?? 0) * 100);
      return [
        `### ${pkg.name} v${pkg.version}`,
        `- **Descrição:** ${(pkg.description ?? "—").slice(0, 120)}`,
        `- **Qualidade:** ${quality}% | **Manutenção:** ${maintenance}%`,
        `- **Links:** [npm](https://www.npmjs.com/package/${pkg.name})${pkg.links?.repository ? ` | [repo](${pkg.links.repository})` : ""}`,
        `- **Instalação:** \`npm install ${pkg.name}\``,
        "",
      ].join("\n");
    });

    return {
      content: [
        {
          type: "text",
          text: `## Resultados NPM para "${query}"\n\n${lines.join("\n")}`,
        },
      ],
    };
  }
);

// ── Ferramenta 2: get_npm_versions ─────────────────────────────────────────

server.tool(
  "get_npm_versions",
  "Lista versões estáveis disponíveis de um pacote NPM específico, da mais recente para a mais antiga.",
  {
    packageName: z.string().describe("Nome do pacote (ex: 'react-hook-form', 'zod', '@tanstack/react-query')"),
    limit: z.number().min(1).max(30).default(10).describe("Número máximo de versões a retornar"),
  },
  async ({ packageName, limit }) => {
    const encoded = encodeURIComponent(packageName).replace("%40", "@").replace("%2F", "/");
    const data = await fetchJson(`${NPM_REGISTRY}/${encoded}`);

    const allVersions = Object.keys(data.versions ?? {}).reverse();
    const stable = allVersions
      .filter((v) => !v.includes("-alpha") && !v.includes("-beta") && !v.includes("-rc") && !v.includes("-next"))
      .slice(0, limit);

    if (!stable.length) {
      return { content: [{ type: "text", text: `Nenhuma versão estável encontrada para "${packageName}".` }] };
    }

    const latest = data["dist-tags"]?.latest ?? stable[0];
    const lines = stable.map((v, i) =>
      v === latest ? `🟢 **${v}** (latest)` : `${i + 1}. \`${v}\``
    );

    return {
      content: [
        {
          type: "text",
          text: [
            `## Versões de ${packageName}`,
            ``,
            ...lines,
            ``,
            `**Instalação:**`,
            `\`\`\`bash`,
            `npm install ${packageName}`,
            `# ou versão específica:`,
            `npm install ${packageName}@${latest}`,
            `\`\`\``,
          ].join("\n"),
        },
      ],
    };
  }
);

// ── Ferramenta 3: check_npm_compat ─────────────────────────────────────────

server.tool(
  "check_npm_compat",
  "Verifica se um pacote NPM é compatível com Next.js 15 e React 19. Analisa peerDependencies e reporta conflitos.",
  {
    packageName: z.string().describe("Nome do pacote (ex: 'react-hook-form', '@tanstack/react-query')"),
    version: z.string().optional().describe("Versão a verificar (deixe em branco para usar a latest)"),
  },
  async ({ packageName, version }) => {
    const encoded = encodeURIComponent(packageName).replace("%40", "@").replace("%2F", "/");
    const data = await fetchJson(`${NPM_REGISTRY}/${encoded}`);

    const targetVersion = version ?? data["dist-tags"]?.latest;
    if (!targetVersion) {
      return { content: [{ type: "text", text: `Pacote "${packageName}" não encontrado.` }] };
    }

    const versionData = data.versions?.[targetVersion];
    if (!versionData) {
      return {
        content: [{ type: "text", text: `Versão ${targetVersion} de "${packageName}" não encontrada.` }],
      };
    }

    const { peerDependencies = {}, dependencies = {}, engines = {} } = versionData;
    const { issues, ok } = analyzePeerDeps(peerDependencies);

    const nodeEngine = engines?.node ?? "não especificado";
    const compatible = issues.length === 0;
    const status = compatible ? "✅ COMPATÍVEL" : "⚠️ POSSÍVEIS CONFLITOS";

    const peerList = Object.entries(peerDependencies)
      .map(([k, v]) => `  - \`${k}\`: \`${v}\``)
      .join("\n");

    const depCount = Object.keys(dependencies).length;

    return {
      content: [
        {
          type: "text",
          text: [
            `## ${packageName} v${targetVersion} — ${status}`,
            ``,
            `**Ambiente alvo:** Next.js 15 + React 19 + Node.js 22`,
            `**Node.js requerido:** ${nodeEngine}`,
            `**Dependências:** ${depCount} pacotes`,
            ``,
            peerList
              ? `**Peer Dependencies:**\n${peerList}`
              : `**Peer Dependencies:** nenhuma`,
            ``,
            ok.length ? `**Compatibilidades verificadas:**\n${ok.map((x) => `  - ${x}`).join("\n")}` : "",
            issues.length
              ? `\n⚠️ **Conflitos detectados:**\n${issues.map((x) => `  - ${x}`).join("\n")}\n\n> Verifique a documentação do pacote ou considere uma alternativa.`
              : `\n> ✅ Nenhum conflito de peer dependency com Next.js 15 / React 19 detectado.`,
            ``,
            `**Instalação:**`,
            `\`\`\`bash`,
            `npm install ${packageName}@${targetVersion}`,
            `\`\`\``,
          ]
            .filter(Boolean)
            .join("\n"),
        },
      ],
    };
  }
);

// ── Iniciar servidor ────────────────────────────────────────────────────────

const transport = new StdioServerTransport();
await server.connect(transport);
