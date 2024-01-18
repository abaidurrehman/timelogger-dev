import { ProjectApi } from './project-api';
import { HttpHelper } from './http-helper';
import { Project, ApiResponse, ProjectStatus } from '../shared/types';


const sampleProjects: Project[] = [
    { id: 1, name: 'Project 1', deadline: '2024-01-31', status: ProjectStatus.New },
    { id: 2, name: 'Project 2', deadline: '2024-02-15', status: ProjectStatus.InProgress },
];

const newProject: Project = {
    id: 3,
    name: 'New Project',
    deadline: '2024-03-01',
    status: ProjectStatus.New,
};

jest.mock('./http-helper', () => ({
    HttpHelper: {
        makeApiRequest: jest.fn(),
    },
}));

describe('ProjectApi', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    describe('getProjects', () => {
        it('should make a GET request to the correct endpoint and return projects', async () => {
            // Arrange
            const expectedEndpoint = '/projects';
            (HttpHelper.makeApiRequest as jest.Mock).mockResolvedValue(sampleProjects);

            // Act
            const result = await ProjectApi.getProjects();

            // Assert
            expect(HttpHelper.makeApiRequest).toHaveBeenCalledWith(
                expectedEndpoint,
                'GET'
            );
            expect(result).toEqual(sampleProjects);
        });
    });

    describe('registerProject', () => {
        it('should make a POST request to the correct endpoint with the provided new project', async () => {
            // Arrange
            const expectedEndpoint = '/projects';
            const successApiResponse: ApiResponse = { message: 'Project registered successfully' };
            (HttpHelper.makeApiRequest as jest.Mock).mockResolvedValue(successApiResponse);

            // Act
            const result = await ProjectApi.registerProject(newProject);

            // Assert
            expect(HttpHelper.makeApiRequest).toHaveBeenCalledWith(
                expectedEndpoint,
                'POST',
                newProject
            );
            expect(result).toEqual(successApiResponse);
        });
    });
});
