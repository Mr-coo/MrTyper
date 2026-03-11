import api from "./api.service"

export const login = async (data: { username: string; password: string }) => {
  console.log(data);
  const response = await api.post("/User/login", data)
  return response.data
}

export const register = async (data: { name:string; username: string; password: string }) => {
  const response = await api.post("/User/register", data)
  return response.data
}