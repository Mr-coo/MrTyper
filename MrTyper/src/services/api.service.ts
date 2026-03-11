import axios from "axios"
import { toast } from "vue-sonner"

const api = axios.create({
  baseURL: "https://localhost:32769/api",
  headers: {
    "Content-Type": "application/json"
  }
})

api.interceptors.response.use(
  (response) => {
    console.log(response.data)
    toast.info(response.data)
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