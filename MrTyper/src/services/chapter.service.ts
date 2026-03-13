import api from "./api.service"

export const all = async () => {
  const response = await api.get("/Chapter")
  return response.data
}

export const create = async (data : { name: string, isPrivate: boolean }) => {
  const response = await api.post("/Chapter", data)
  return response.data
}

export const del = async (chapterId : string) => {
  const response = await api.delete("/Chapter/" + chapterId)
  return response.data
}

export const update = async (chapterId : string, data : { name: string, isPrivate: boolean }) => {
  const response = await api.put("/Chapter/" + chapterId, data)
  return response.data
}