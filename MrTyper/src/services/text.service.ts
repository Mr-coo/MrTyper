import api from './api.service'

export const getByTextId = async (chapterId: string) => {
  const response = await api.get('/Text/chapter-id/' + chapterId)
  return response.data
}

export const create = async (chapterId: string, data: { content: string }) => {
  const response = await api.post(`/Text?chapterId=${chapterId}`, data.content)
  return response.data
}

export const del = async (TextId: string) => {
  const response = await api.delete('/Text/' + TextId)
  return response.data
}

export const update = async (TextId: string, data: { content: string }) => {
  const response = await api.put('/Text/' + TextId, data.content)
  return response.data
}
