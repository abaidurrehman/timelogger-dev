import axios from 'axios';
import { ProjectApi } from './project-api';
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

jest.mock('axios');

describe('ProjectApi', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    describe('getProjects', () => {
        it('should make a GET request to the correct endpoint and return projects', async () => {
            // Arrange
            const expectedEndpoint = '/projects';
            (axios.get as jest.Mock).mockResolvedValue({ data: sampleProjects });

            // Act
            const result = await ProjectApi.getProjects();

            // Assert
            expect(axios.get).toHaveBeenCalledWith(
                expect.stringContaining(expectedEndpoint),
                expect.any(Object)
            );
            expect(result).toEqual(sampleProjects);
        });
    });

    describe('addProject', () => {
        it('should make a POST request to the correct endpoint with the provided new project', async () => {
            // Arrange
            const expectedEndpoint = '/projects';
            const successApiResponse: ApiResponse = { message: 'Project registered successfully' };
            (axios.post as jest.Mock).mockResolvedValue({ data: successApiResponse });

            // Act
            const result = await ProjectApi.addProject(newProject);

            // Assert
            expect(axios.post).toHaveBeenCalledWith(
                expect.stringContaining(expectedEndpoint),
                newProject,
                expect.any(Object)
            );
            expect(result).toEqual(successApiResponse);
        });
    });
});
