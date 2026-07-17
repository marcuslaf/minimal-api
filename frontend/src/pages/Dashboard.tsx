import { useEffect, useState } from "react"
import { healthService, veiculoService, administradorService } from "@/services/services"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Car, Users, Activity, AlertCircle } from "lucide-react"

export default function Dashboard() {
  const [health, setHealth] = useState<string>("verificando...")
  const [veiculosCount, setVeiculosCount] = useState(0)
  const [admsCount, setAdmsCount] = useState(0)
  const [healthOk, setHealthOk] = useState(true)

  useEffect(() => {
    async function load() {
      try {
        await healthService.check()
        setHealth("Online")
        setHealthOk(true)
      } catch {
        setHealth("Offline")
        setHealthOk(false)
      }
      try {
        const v = await veiculoService.listar()
        setVeiculosCount(v.length)
      } catch { /* */ }
      try {
        const a = await administradorService.listar()
        setAdmsCount(a.length)
      } catch { /* */ }
    }
    load()
  }, [])

  const stats = [
    { title: "Status da API", value: health, icon: healthOk ? Activity : AlertCircle, color: healthOk ? "text-green-600" : "text-red-600" },
    { title: "Veículos Cadastrados", value: veiculosCount.toString(), icon: Car, color: "text-blue-600" },
    { title: "Administradores", value: admsCount.toString(), icon: Users, color: "text-purple-600" },
  ]

  return (
    <div className="space-y-6">
      <h1 className="text-3xl font-bold">Dashboard</h1>
      <div className="grid gap-4 md:grid-cols-3">
        {stats.map((s) => (
          <Card key={s.title}>
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium">{s.title}</CardTitle>
              <s.icon className={`h-5 w-5 ${s.color}`} />
            </CardHeader>
            <CardContent>
              <div className="text-3xl font-bold">{s.value}</div>
            </CardContent>
          </Card>
        ))}
      </div>
    </div>
  )
}
