import { useEffect, useState } from "react"
import { veiculoService } from "@/services/services"
import { useAuth } from "@/contexts/AuthContext"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { toast } from "@/components/ui/use-toast"
import type { Veiculo } from "@/types"
import { Plus, Pencil, Trash2 } from "lucide-react"

const marcas = ["Toyota", "Honda", "Ford", "Chevrolet", "Volkswagen", "Hyundai", "Fiat", "Nissan", "BMW", "Mercedes-Benz"]

export default function Veiculos() {
  const { isAdm } = useAuth()
  const [veiculos, setVeiculos] = useState<Veiculo[]>([])
  const [loading, setLoading] = useState(true)
  const [dialogOpen, setDialogOpen] = useState(false)
  const [editingId, setEditingId] = useState<number | null>(null)
  const [form, setForm] = useState({ nome: "", marca: "", ano: new Date().getFullYear() })
  const [filtro, setFiltro] = useState({ nome: "", marca: "" })

  const load = async () => {
    setLoading(true)
    try {
      const data = await veiculoService.listar(undefined, filtro.nome || undefined, filtro.marca || undefined)
      setVeiculos(data)
    } catch {
      toast({ title: "Erro", description: "Falha ao carregar veículos", variant: "destructive" })
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { load() }, [])

  const openCreate = () => {
    setEditingId(null)
    setForm({ nome: "", marca: "", ano: new Date().getFullYear() })
    setDialogOpen(true)
  }

  const openEdit = (v: Veiculo) => {
    setEditingId(v.id)
    setForm({ nome: v.nome, marca: v.marca, ano: v.ano })
    setDialogOpen(true)
  }

  const handleSave = async () => {
    try {
      if (editingId) {
        await veiculoService.atualizar(editingId, form)
        toast({ title: "Sucesso", description: "Veículo atualizado!" })
      } else {
        await veiculoService.criar(form)
        toast({ title: "Sucesso", description: "Veículo criado!" })
      }
      setDialogOpen(false)
      load()
    } catch (err: unknown) {
      const error = err as { response?: { data?: { mensagens?: string[] } } }
      toast({
        title: "Erro de validação",
        description: error.response?.data?.mensagens?.join(", ") || "Falha ao salvar",
        variant: "destructive",
      })
    }
  }

  const handleDelete = async (id: number) => {
    if (!confirm("Tem certeza que deseja excluir?")) return
    try {
      await veiculoService.deletar(id)
      toast({ title: "Sucesso", description: "Veículo excluído!" })
      load()
    } catch {
      toast({ title: "Erro", description: "Falha ao excluir", variant: "destructive" })
    }
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold">Veículos</h1>
        {isAdm && (
          <Dialog open={dialogOpen} onOpenChange={setDialogOpen}>
            <DialogTrigger asChild>
              <Button onClick={openCreate}>
                <Plus className="mr-2 h-4 w-4" /> Novo Veículo
              </Button>
            </DialogTrigger>
            <DialogContent>
              <DialogHeader>
                <DialogTitle>{editingId ? "Editar" : "Novo"} Veículo</DialogTitle>
                <DialogDescription>
                  {editingId ? "Altere os dados do veículo" : "Preencha os dados para criar um veículo"}
                </DialogDescription>
              </DialogHeader>
              <div className="space-y-4 py-4">
                <div className="space-y-2">
                  <Label>Nome</Label>
                  <Input value={form.nome} onChange={(e) => setForm({ ...form, nome: e.target.value })} placeholder="Ex: Corolla" />
                </div>
                <div className="space-y-2">
                  <Label>Marca</Label>
                  <select
                    className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
                    value={form.marca}
                    onChange={(e) => setForm({ ...form, marca: e.target.value })}
                  >
                    <option value="">Selecione a marca</option>
                    {marcas.map((m) => <option key={m} value={m}>{m}</option>)}
                  </select>
                </div>
                <div className="space-y-2">
                  <Label>Ano</Label>
                  <Input
                    type="number"
                    min={1950}
                    max={new Date().getFullYear() + 1}
                    value={form.ano}
                    onChange={(e) => setForm({ ...form, ano: parseInt(e.target.value) })}
                  />
                </div>
              </div>
              <DialogFooter>
                <Button variant="outline" onClick={() => setDialogOpen(false)}>Cancelar</Button>
                <Button onClick={handleSave}>{editingId ? "Salvar" : "Criar"}</Button>
              </DialogFooter>
            </DialogContent>
          </Dialog>
        )}
      </div>

      <div className="flex gap-4">
        <Input placeholder="Filtrar por nome..." value={filtro.nome} onChange={(e) => setFiltro({ ...filtro, nome: e.target.value })} className="max-w-xs" />
        <select
          className="flex h-10 rounded-md border border-input bg-background px-3 py-2 text-sm"
          value={filtro.marca}
          onChange={(e) => setFiltro({ ...filtro, marca: e.target.value })}
        >
          <option value="">Todas as marcas</option>
          {marcas.map((m) => <option key={m} value={m}>{m}</option>)}
        </select>
        <Button variant="outline" onClick={load}>Buscar</Button>
      </div>

      {loading ? (
        <p className="text-muted-foreground">Carregando...</p>
      ) : veiculos.length === 0 ? (
        <p className="text-muted-foreground">Nenhum veículo encontrado.</p>
      ) : (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {veiculos.map((v) => (
            <Card key={v.id}>
              <CardHeader>
                <CardTitle className="flex items-center justify-between">
                  <span>{v.nome}</span>
                  <div className="flex gap-1">
                    {isAdm && (
                      <>
                        <Button variant="ghost" size="icon" onClick={() => openEdit(v)}>
                          <Pencil className="h-4 w-4" />
                        </Button>
                        <Button variant="ghost" size="icon" onClick={() => handleDelete(v.id)}>
                          <Trash2 className="h-4 w-4 text-red-500" />
                        </Button>
                      </>
                    )}
                  </div>
                </CardTitle>
              </CardHeader>
              <CardContent>
                <p><strong>Marca:</strong> {v.marca}</p>
                <p><strong>Ano:</strong> {v.ano}</p>
              </CardContent>
            </Card>
          ))}
        </div>
      )}
    </div>
  )
}
