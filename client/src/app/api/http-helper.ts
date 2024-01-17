const BASE_URL = "http://localhost:3001/api";

const HEADERS = {
    'Content-Type': 'application/json',
};

export class HttpHelper {
    static async handleResponse(response: Response) {

        return response.json();
    }

    static async makeApiRequest(url: string, method: string, body?: object) {
        try {
            const response = await fetch(`${BASE_URL}${url}`, {
                method,
                headers: HEADERS,
                body: body ? JSON.stringify(body) : undefined,
            });

            return this.handleResponse(response);
        } catch (error: any) {
            console.error('API Request Error:', error.message);
            throw new Error('An error occurred while processing the request.');
        }
    }
}