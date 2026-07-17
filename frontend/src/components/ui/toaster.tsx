import { useToast } from "@/components/ui/use-toast"
import { Toast, ToastDescription, ToastTitle } from "@/components/ui/toast"

export function Toaster() {
  const { toasts } = useToast()

  return (
    <div className="fixed bottom-4 right-4 z-[100] flex flex-col gap-2">
      {toasts.map(({ id, title, description, variant }) => (
        <Toast key={id} variant={variant} data-state="open">
          <div className="grid gap-1">
            {title && <ToastTitle>{title}</ToastTitle>}
            {description && <ToastDescription>{description}</ToastDescription>}
          </div>
        </Toast>
      ))}
    </div>
  )
}
