import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import ProjectTable from './ProjectTable';
import { ProjectStatus } from '../shared/types';
import { ProjectApi } from '../api/project-api';

jest.mock('../api/project-api', () => ({
    ProjectApi: {
        getProjects: jest.fn(() => Promise.resolve([])),
    },
}));

describe('ProjectTable', () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    test('renders projects table with rows', async () => {
        const mockProjects = [
            { id: 1, name: 'Project 1', deadline: '2024-01-30', status: ProjectStatus.InProgress },
            { id: 2, name: 'Project 2', deadline: '2024-02-15', status: ProjectStatus.Complete },
        ];

        jest.spyOn(ProjectApi, 'getProjects').mockResolvedValueOnce(mockProjects);

        render(<ProjectTable />);

        await screen.findByText('Projects');

        expect(screen.getAllByRole('row')).toHaveLength(mockProjects.length);
    });

});
