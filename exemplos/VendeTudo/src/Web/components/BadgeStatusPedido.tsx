import { Badge } from "@/components/ui/badge";

const STATUS_LABELS: Record<number, string> = {
  1: "Submetido",
  2: "Aguardando Validação",
  3: "Estoque Confirmado",
  4: "Pago",
  5: "Enviado",
  6: "Cancelado",
};

const STATUS_VARIANTS: Record<number, "default" | "secondary" | "destructive" | "outline" | "success" | "warning"> = {
  1: "secondary",
  2: "warning",
  3: "default",
  4: "success",
  5: "success",
  6: "destructive",
};

export function BadgeStatusPedido({ status }: { status: number }) {
  return (
    <Badge variant={STATUS_VARIANTS[status] ?? "outline"}>
      {STATUS_LABELS[status] ?? "Desconhecido"}
    </Badge>
  );
}
