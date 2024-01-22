import axios, { AxiosResponse } from 'axios';
import { ApiResponse, Project } from "../shared/types";

const BASE_URL = "http://localhost:3001/api";
const API_ENDPOINT_PROJECTS = "/projects";

const HEADERS = {
    'Content-Type': 'application/json',
};

export class ProjectApi {
    static async getProjects(): Promise<Project[]> {
        try {
            const response: AxiosResponse<Project[]> = await axios.get<Project[]>(`${BASE_URL}${API_ENDPOINT_PROJECTS}`, { headers: HEADERS });
            return response.data;
        } catch (error:any) {
            throw new Error(`Error fetching projects: ${error.message}`);
        }
    }

    static async addProject(newProject: Project): Promise<ApiResponse> {
        try {
            const response: AxiosResponse<ApiResponse> = await axios.post<ApiResponse>(`${BASE_URL}${API_ENDPOINT_PROJECTS}`, newProject, { headers: HEADERS });
            return response.data;
        } catch (error:any) {
            throw new Error(`Error registering project: ${error.message}`);
        }
    }
}
