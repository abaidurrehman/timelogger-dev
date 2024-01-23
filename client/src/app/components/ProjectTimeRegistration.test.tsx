import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import ProjectTimeRegistration from './ProjectTimeRegistration';
import { TimeRegistrationApi } from '../api/time-registration-api';

jest.mock('../api/time-registration-api', () => ({
    TimeRegistrationApi: {
        getTimesForProject: jest.fn(() => Promise.resolve([])),
    },
}));

describe('ProjectTimeRegistration component', () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    it('fetches time registrations for the project on mount', async () => {
        // Arrange
        const projectId = 1;
        const mockTimeRegistrations = [
            {
                id: 1,
                projectId: 1,
                FreelancerId: 123,
                taskDescription: 'Task 1',
                date: '2024-01-30',
                startTime: '2024-01-30T09:00:00',
                endTime: '2024-01-30T12:30:00'
            },
            {
                id: 2,
                projectId: 1,
                FreelancerId: 1,
                taskDescription: 'Task 2',
                date: '2024-01-30',
                startTime: '2024-01-30T13:00:00',
                endTime: '2024-01-30T17:30:00'
            },
        ];

        jest.spyOn(TimeRegistrationApi, 'getTimesForProject').mockResolvedValueOnce(mockTimeRegistrations);

        // Act
        render(<ProjectTimeRegistration projectId={projectId} />);

        await waitFor(() => {
            // Assert
            expect(TimeRegistrationApi.getTimesForProject).toHaveBeenCalledWith(projectId);
            expect(screen.getByText(/Task 1/i)).toBeInTheDocument();
            expect(screen.getByText(/Task 2/i)).toBeInTheDocument();
        });
    });

    it('opens TimeRegistrationForm on "Register Time" button click', async () => {
        // Arrange
        const projectId = 1;
        const mockTimeRegistrations = [
            {
                id: 1,
                projectId: 1,
                FreelancerId: 123,
                taskDescription: 'Task 1',
                date: '2024-01-30',
                startTime: '2024-01-30T09:00:00',
                endTime: '2024-01-30T12:30:00'
            },
            {
                id: 2,
                projectId: 1,
                FreelancerId: 1,
                taskDescription: 'Task 2',
                date: '2024-01-30',
                startTime: '2024-01-30T13:00:00',
                endTime: '2024-01-30T17:30:00'
            },
        ];

        jest.spyOn(TimeRegistrationApi, 'getTimesForProject').mockResolvedValueOnce(mockTimeRegistrations);

        // Act
        render(<ProjectTimeRegistration projectId={projectId} />);

        fireEvent.click(screen.getByText(/Register Time/i));

        await waitFor(() => {
            // Assert
            expect(screen.getByTestId('time-registration-form')).toBeInTheDocument();
        });
    });
});
