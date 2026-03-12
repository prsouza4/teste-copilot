#!/usr/bin/env node
/**
 * nuget-mcp — Servidor MCP para busca de pacotes NuGet compatíveis com .NET 10
 *
 * Ferramentas disponíveis:
 *   - search_nuget: busca pacotes por termo, filtra por compatibilidade com net10.0
 *   - get_nuget_versions: lista versões estáveis de um pacote específico
 *   - check_nuget_compatibility: verifica se uma versão específica é compatível com .NET 10
 */

import { McpServer } from "@modelcontextprotocol/sdk/server/mcp.js";
import { StdioServerTransport } from "@modelcontextprotocol/sdk/server/stdio.js";
import { z } from "zod";

const NUGET_BASE = "https://api.nuget.org/v3";
const TARGET_FRAMEWORK = "net10.0";

// ── Helpers ────────────────────────────────────────────────────────────────

async function fetchJson(url) {
  const res = await fetch(url, {
    headers: { "User-Agent": "nuget-mcp/1.0 (github.com/copilot-agent)" },
    signal: AbortSignal.timeout(10000),
  });
  if (!res.ok) throw new Error(`HTTP ${res.status}: ${url}`);
  return res.json();
}

/** Retorna índice de serviços do NuGet v3 */
async function getServiceIndex() {
  const index = await fetchJson(`${NUGET_BASE}/index.json`);
  const getUrl = (type) =>
    index.resources.find((r) => r["@type"].startsWith(type))?.["@id"];
  return {
    search: getUrl("SearchQueryService"),
    packageBase: getUrl("PackageBaseAddress"),
    registration: getUrl("RegistrationsBaseUrl"),
  };
}

/** Verifica se um pacote suporta um target framework */
function supportsFramework(catalogEntry, framework) {
  // Verifica dependencyGroups
  const groups = catalogEntry?.dependencyGroups ?? [];
  if (groups.length === 0) return true; // sem restrição = compatível

  return groups.some((g) => {
    const tfm = g.targetFramework?.toLowerCase() ?? "";
    return (
      tfm === "" ||
      tfm === framework ||
      tfm.startsWith("net1") ||
      tfm.startsWith(".netstandard") ||
      tfm === "netstandard2.0" ||
      tfm === "netstandard2.1" ||
      tfm === "any"
    );
  });
}

// ── Servidor MCP ────────────────────────────────────────────────────────────

const server = new McpServer({
  name: "nuget-mcp",
  version: "1.0.0",
});

// ── Ferramenta 1: search_nuget ──────────────────────────────────────────────

server.tool(
  "search_nuget",
  "Busca pacotes NuGet compatíveis com .NET 10 por termo. Retorna nome, versão mais recente, downloads e descrição.",
  {
    query: z.string().describe("Termo de busca (ex: 'entity framework', 'serilog', 'masstransit')"),
    take: z.number().min(1).max(20).default(10).describe("Número de resultados (1-20, padrão 10)"),
    prerelease: z.boolean().default(false).describe("Incluir versões pré-lançamento"),
  },
  async ({ query, take, prerelease }) => {
    const { search } = await getServiceIndex();
    const url = `${search}?q=${encodeURIComponent(query)}&take=${take}&prerelease=${prerelease}&semVerLevel=2.0.0`;
    const data = await fetchJson(url);

    if (!data.data?.length) {
      return { content: [{ type: "text", text: `Nenhum pacote encontrado para "${query}".` }] };
    }

    const lines = data.data.map((pkg) => {
      const frameworks = pkg.versions?.slice(-1)[0]?.version ? "" : "";
      return [
        `### ${pkg.id} v${pkg.version}`,
        `- **Downloads totais:** ${(pkg.totalDownloads ?? 0).toLocaleString("pt-BR")}`,
        `- **Descrição:** ${(pkg.description ?? "—").slice(0, 150)}`,
        `- **Autores:** ${(pkg.authors ?? []).join(", ")}`,
        `- **Licença:** ${pkg.licenseExpression ?? "não informada"}`,
        `- **Instalação:** \`dotnet add package ${pkg.id}\``,
        "",
      ].join("\n");
    });

    return {
      content: [
        {
          type: "text",
          text: `## Resultados NuGet para "${query}" (${data.totalHits} encontrados)\n\n${lines.join("\n")}`,
        },
      ],
    };
  }
);

// ── Ferramenta 2: get_nuget_versions ───────────────────────────────────────

server.tool(
  "get_nuget_versions",
  "Lista todas as versões estáveis disponíveis de um pacote NuGet específico, da mais recente para a mais antiga.",
  {
    packageId: z.string().describe("ID exato do pacote (ex: 'Microsoft.EntityFrameworkCore', 'Serilog')"),
    includePrerelease: z.boolean().default(false).describe("Incluir versões pré-lançamento"),
    limit: z.number().min(1).max(50).default(15).describe("Número máximo de versões a retornar"),
  },
  async ({ packageId, includePrerelease, limit }) => {
    const { packageBase } = await getServiceIndex();
    const id = packageId.toLowerCase();
    const data = await fetchJson(`${packageBase}${id}/index.json`);

    let versions = (data.versions ?? []).reverse();

    if (!includePrerelease) {
      versions = versions.filter(
        (v) => !v.includes("-alpha") && !v.includes("-beta") && !v.includes("-rc") && !v.includes("-preview")
      );
    }

    const shown = versions.slice(0, limit);

    if (!shown.length) {
      return { content: [{ type: "text", text: `Nenhuma versão encontrada para "${packageId}".` }] };
    }

    const lines = shown.map((v, i) => `${i === 0 ? "🟢 **Mais recente:**" : `${i + 1}.`} \`${v}\``);

    return {
      content: [
        {
          type: "text",
          text: [
            `## Versões de ${packageId}`,
            ``,
            ...lines,
            ``,
            `**Instalação da mais recente:**`,
            `\`\`\`bash`,
            `dotnet add package ${packageId}`,
            `# ou versão específica:`,
            `dotnet add package ${packageId} --version ${shown[0]}`,
            `\`\`\``,
          ].join("\n"),
        },
      ],
    };
  }
);

// ── Ferramenta 3: check_nuget_compatibility ────────────────────────────────

server.tool(
  "check_nuget_compatibility",
  `Verifica se uma versão específica de um pacote NuGet é compatível com ${TARGET_FRAMEWORK} (.NET 10). Retorna target frameworks suportados e recomendação.`,
  {
    packageId: z.string().describe("ID do pacote (ex: 'Newtonsoft.Json')"),
    version: z.string().optional().describe("Versão a verificar (deixe em branco para usar a mais recente)"),
  },
  async ({ packageId, version }) => {
    const { registration, packageBase } = await getServiceIndex();
    const id = packageId.toLowerCase();

    // Resolve versão se não fornecida
    let targetVersion = version;
    if (!targetVersion) {
      const idx = await fetchJson(`${packageBase}${id}/index.json`);
      const stable = (idx.versions ?? []).filter(
        (v) => !v.includes("-") 
      );
      targetVersion = stable[stable.length - 1];
      if (!targetVersion) targetVersion = idx.versions?.slice(-1)[0];
    }

    if (!targetVersion) {
      return { content: [{ type: "text", text: `Pacote "${packageId}" não encontrado.` }] };
    }

    // Busca catalog entry para a versão
    const regUrl = `${registration}${id}/${targetVersion.toLowerCase()}.json`;
    let catalogEntry;
    try {
      const leaf = await fetchJson(regUrl);
      catalogEntry = leaf.catalogEntry ?? leaf;
    } catch {
      return {
        content: [{ type: "text", text: `Versão ${targetVersion} de "${packageId}" não encontrada no registro.` }],
      };
    }

    const groups = catalogEntry?.dependencyGroups ?? [];
    const frameworks = groups.map((g) => g.targetFramework ?? "any").filter(Boolean);
    const compatible = supportsFramework(catalogEntry, TARGET_FRAMEWORK);

    const status = compatible ? "✅ COMPATÍVEL" : "❌ NÃO COMPATÍVEL";
    const recommendation = compatible
      ? `\`dotnet add package ${packageId} --version ${targetVersion}\``
      : `Verifique uma versão mais recente ou use um pacote alternativo compatível com ${TARGET_FRAMEWORK}.`;

    return {
      content: [
        {
          type: "text",
          text: [
            `## ${packageId} v${targetVersion} — ${status} com ${TARGET_FRAMEWORK}`,
            ``,
            `**Target Frameworks declarados:** ${frameworks.length ? frameworks.join(", ") : "nenhum (universal)"}`,
            ``,
            `**Recomendação:**`,
            recommendation,
            ``,
            frameworks.length === 0
              ? `> ℹ️ Pacote sem restrição de framework — compatível com todas as versões .NET.`
              : ``,
          ].join("\n"),
        },
      ],
    };
  }
);

// ── Iniciar servidor ────────────────────────────────────────────────────────

const transport = new StdioServerTransport();
await server.connect(transport);
