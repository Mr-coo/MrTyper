import axios from "axios"
import { toast } from "vue-sonner"

const api = axios.create({
  baseURL: "https://localhost:32771/api",
  headers: {
    "Content-Type": "application/json"
  }
})

api.interceptors.response.use(
  (response) => {
    toast.info(response.data.message)
    return response
  },
  (error) => {
    const message =
      error.response?.data?.message ||
      error.response?.data?.title ||
      "Something went wrong"

    toast.error(message)

    return Promise.reject(error)
  }
)

export default api