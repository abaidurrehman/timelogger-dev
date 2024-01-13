import { Project } from "../shared/types";

const BASE_URL = "http://localhost:3001/api";

const projectApi = {
    getProjects: async () => {
        try {
            const response = await fetch(`${BASE_URL}/projects`);

            if (!response.ok) {
                throw new Error(`Error: ${response.status}`);
            }

            const data = await response.json();
            return data;
        } catch (error: any) {
            console.error('Error fetching data:', error.message);
            throw error;
        }
    },

    registerProject: async (newProject: Project) => {
        try {
            const response = await fetch(`${BASE_URL}/projects`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(newProject),
            });

            if (!response.ok) {
                throw new Error(`Error: ${response.status}`);
            }

            const data = await response.json();
            return data;
        } catch (error: any) {
            console.error('Error registering project:', error.message);
            throw error;
        }
    },
};

export default projectApi;