import axios from "axios"
import Cookies from 'js-cookie'
import { toast } from "vue-sonner"

const api = axios.create({
  baseURL: "https://localhost:32771/api",
  headers: {
    "Content-Type": "application/json"
  }
})

api.interceptors.request.use(
  (config) => {
    const token = Cookies.get('access_token'); 
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);


api.interceptors.response.use(
  (response) => {
    if(response.data.message) toast.info(response.data.message)
    return response
  },
  (error) => {
    if(error.response.status == 401){
      const message = "Who are you??"
      toast.error(message)

      window.location.href = '/login'

      return
    }

    const message =
      error.response?.data?.message ||
      error.response?.data?.title ||
      "Something went wrong"

    toast.error(message)

    return Promise.reject(error)
  }
)

export default api