export interface Chapter {
  id: string;
  name: string;
  isPrivate: boolean;
  userId: string;
}

export interface Text {
  id: string;
  content: string;
  chapterId: string;
}