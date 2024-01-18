import { ApiResponse, TimeRegistration } from "../shared/types";
import { HttpHelper } from "./http-helper";

export class TimeRegistrationApi {
    private static API_ENDPOINT_TIMEREGISTRATIONS = "/timeregistration";

    static async addTimeRegistration(timeRegistration: TimeRegistration): Promise<ApiResponse> {
        return HttpHelper.makeApiRequest(this.API_ENDPOINT_TIMEREGISTRATIONS, 'POST', timeRegistration);
    }

    static async getTimesForProject(projectId: number): Promise<TimeRegistration[]> {
        const url = `${this.API_ENDPOINT_TIMEREGISTRATIONS}/GetTimesForProject/${projectId}`;
        return HttpHelper.makeApiRequest(url, 'GET');
    }

    static async checkDuplicateTime(timeRegistration: TimeRegistration): Promise<boolean> {
        try {
            const response = await HttpHelper.makeApiRequest(this.API_ENDPOINT_TIMEREGISTRATIONS, 'POST', timeRegistration);
            return response.message.toLowerCase().includes('duplicate');
        } catch (error) {
            console.error('Error checking duplicate time registration:', error);
            return false;
        }
    }
}