import { ApiResponse, Project } from "../shared/types";
import { HttpHelper } from "./http-helper";

export class ProjectApi {
    private static API_ENDPOINT_PROJECTS = "/projects";

    static async getProjects(): Promise<Project[]> {
        return HttpHelper.makeApiRequest(this.API_ENDPOINT_PROJECTS, 'GET');
    }

    static async registerProject(newProject: Project): Promise<ApiResponse> {
        return HttpHelper.makeApiRequest(this.API_ENDPOINT_PROJECTS, 'POST', newProject);
    }
}