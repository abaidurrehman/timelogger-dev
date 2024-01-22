import axios, { AxiosResponse } from 'axios';
import { ApiResponse, TimeRegistration } from "../shared/types";

const BASE_URL = "http://localhost:3001/api";
const API_ENDPOINT_TIMEREGISTRATIONS = "/timeregistration";

const HEADERS = {
    'Content-Type': 'application/json',
};

export class TimeRegistrationApi {
    static async addTimeRegistration(timeRegistration: TimeRegistration): Promise<ApiResponse> {
        try {
            const response: AxiosResponse<ApiResponse> = await axios.post<ApiResponse>(
                `${BASE_URL}${API_ENDPOINT_TIMEREGISTRATIONS}`,
                timeRegistration,
                { headers: HEADERS }
            );
            return response.data;
        } catch (error:any) {
            throw new Error(`Error adding time registration: ${error.message}`);
        }
    }

    static async getTimesForProject(projectId: number): Promise<TimeRegistration[]> {
        try {
            const url = `${BASE_URL}${API_ENDPOINT_TIMEREGISTRATIONS}/GetTimesForProject/${projectId}`;
            const response: AxiosResponse<TimeRegistration[]> = await axios.get<TimeRegistration[]>(url, { headers: HEADERS });
            return response.data;
        } catch (error:any) {
            throw new Error(`Error fetching times for project: ${error.message}`);
        }
    }

    static async checkDuplicateTime(timeRegistration: TimeRegistration): Promise<boolean> {
        try {
            const response: AxiosResponse<ApiResponse> = await axios.post<ApiResponse>(
                `${BASE_URL}${API_ENDPOINT_TIMEREGISTRATIONS}`,
                timeRegistration,
                { headers: HEADERS }
            );
            return response.data.message.toLowerCase().includes('duplicate');
        } catch (error:any) {
            console.error('Error checking duplicate time registration:', error);
            return false;
        }
    }
}
