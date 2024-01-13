import { ApiResponse, TimeRegistration } from "../shared/types";

const BASE_URL = "http://localhost:3001/api";

const timeregistrationApi = {
    addTimeRegistration: async (formData: TimeRegistration): Promise<ApiResponse> => {
        try {
            const response = await fetch(`${BASE_URL}/timeregistrations`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
            });

            if (!response.ok) {
                const data = await response.json();
                throw new Error(data.message);
            }

            const data = await response.json();
            return data;
        } catch (error) {
            console.error('Error submitting time registration:', error);
            throw new Error('Error submitting time registration.');
        }
    },

    getTimesForProject: async (projectId: number): Promise<TimeRegistration[]> => {
        try {
            const response = await fetch(`${BASE_URL}/timeregistrations/GetTimesForProject/${projectId}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (!response.ok) {
                const data = await response.json();
                throw new Error(data.message);
            }

            const data = await response.json();
            return data;
        } catch (error) {
            console.error(`Error fetching time registrations for project ${projectId}:`, error);
            throw new Error(`Error fetching time registrations for project ${projectId}.`);
        }
    },
    
    checkDuplicateTime: async (formData: TimeRegistration): Promise<boolean> => {
        try {
            const response = await fetch(`${BASE_URL}/timeregistrations`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
            });

            if (!response.ok) {
                const data = await response.json();
                return data.message.toLowerCase().includes('duplicate');
            }

            return false;
        } catch (error) {
            console.error('Error checking duplicate time registration:', error);
            return false;
        }
    }
}

export default timeregistrationApi;