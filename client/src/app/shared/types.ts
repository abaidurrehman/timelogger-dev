// types.ts

export enum ProjectStatus {
    New = 0,
    InProgress = 1,
    Complete = 2,
}

export interface Project {
    id: number;
    name: string;
    deadline: string;
    status: ProjectStatus;
}

export interface TimeRegistration {
    projectId: number;
    taskDescription: string;
    date: string;
    startTime: string;
    endTime: string;
}

export interface ApiResponse {
    message: string;
}