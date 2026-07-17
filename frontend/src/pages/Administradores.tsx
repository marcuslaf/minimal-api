import { useEffect, useState } from "react"
import { administradorService } from "@/services/services"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { toast } from "@/components/ui/use-toast"
import type { Administrador } from "@/types"
import { Plus, Shield } from "lucide-react"

export default function Administradores() {
  const [adms, setAdms] = useState<Administrador[]>([])
  const [loading, setLoading] = useState(true)
  const [dialogOpen, setDialogOpen] = useState(false)
  const [form, setForm] = useState({ email: "", senha: "", perfil: "Editor" })

  const load = async () => {
    setLoading(true)
    try {
      const data = await administradorService.listar()
      setAdms(data)
    } catch {
      toast({ title: "Erro", description: "Falha ao carregar administradores", variant: "destructive" })
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { load() }, [])

  const handleCreate = async () => {
    try {
      await administradorService.criar(form)
      toast({ title: "Sucesso", description: "Administrador criado!" })
      setDialogOpen(false)
      setForm({ email: "", senha: "", perfil: "Editor" })
      load()
    } catch (err: unknown) {
      const error = err as { response?: { data?: { mensagens?: string[] } } }
      toast({
        title: "Erro de validação",
        description: error.response?.data?.mensagens?.join(", ") || "Falha ao criar",
        variant: "destructive",
      })
    }
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold">Administradores</h1>
        <Dialog open={dialogOpen} onOpenChange={setDialogOpen}>
          <DialogTrigger asChild>
            <Button onClick={() => setForm({ email: "", senha: "", perfil: "Editor" })}>
              <Plus className="mr-2 h-4 w-4" /> Novo Administrador
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Novo Administrador</DialogTitle>
              <DialogDescription>Preencha os dados para criar um novo administrador</DialogDescription>
            </DialogHeader>
            <div className="space-y-4 py-4">
              <div className="space-y-2">
                <Label>Email</Label>
                <Input type="email" value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} placeholder="admin@email.com" />
              </div>
              <div className="space-y-2">
                <Label>Senha</Label>
                <Input type="password" value={form.senha} onChange={(e) => setForm({ ...form, senha: e.target.value })} placeholder="Mínimo 6 caracteres" />
              </div>
              <div className="space-y-2">
                <Label>Perfil</Label>
                <select
                  className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
                  value={form.perfil}
                  onChange={(e) => setForm({ ...form, perfil: e.target.value })}
                >
                  <option value="Editor">Editor</option>
                  <option value="Adm">Administrador</option>
                </select>
              </div>
            </div>
            <DialogFooter>
              <Button variant="outline" onClick={() => setDialogOpen(false)}>Cancelar</Button>
              <Button onClick={handleCreate}>Criar</Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </div>

      {loading ? (
        <p className="text-muted-foreground">Carregando...</p>
      ) : (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {adms.map((a) => (
            <Card key={a.id}>
              <CardHeader>
                <CardTitle className="flex items-center gap-2">
                  <Shield className="h-5 w-5" />
                  {a.email}
                </CardTitle>
              </CardHeader>
              <CardContent>
                <p><strong>Perfil:</strong> {a.perfil}</p>
              </CardContent>
            </Card>
          ))}
        </div>
      )}
    </div>
  )
}
