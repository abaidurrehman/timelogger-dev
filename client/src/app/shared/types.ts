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
    id?: number;
    projectId: number;
    FreelancerId: number;
    taskDescription: string;
    date: string;
    startTime: string;
    endTime: string;
}

export interface ApiResponse {
    message: string;
    errors?: string[];
}
