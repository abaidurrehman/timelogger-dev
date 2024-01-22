import React from 'react';
import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import TimeRegistrationForm from './TimeRegistrationForm';
import { ProjectApi } from '../api/project-api';
import { ProjectStatus } from '../shared/types';
import { TimeRegistrationApi } from '../api/time-registration-api';

jest.mock('../api/project-api', () => ({
  ProjectApi: {
    getProjects: jest.fn(() => Promise.resolve([])),
  },
}));

jest.mock('../api/time-registration-api', () => ({
  TimeRegistrationApi: {
    addTimeRegistration: jest.fn(() => Promise.resolve({ message: 'Time registration added successfully' })),
  },
}));

describe('TimeRegistrationForm', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('submits form successfully', async () => {
    // Arrange
    const mockProjects = [
      { id: 1, name: 'Project 1', deadline: '2024-01-30', status: ProjectStatus.InProgress },
      { id: 2, name: 'Project 2', deadline: '2024-02-15', status: ProjectStatus.Complete },
    ];

    jest.spyOn(ProjectApi, 'getProjects').mockResolvedValueOnce(mockProjects);
    jest.spyOn(TimeRegistrationApi, 'addTimeRegistration').mockResolvedValueOnce({ message: 'Time registration added successfully' });

    // Act
    render(<TimeRegistrationForm onCloseForm={() => { }} onSuccessfulSubmit={() => { }} />);

    await waitFor(() => {
      // Assert
      expect(screen.getByLabelText(/Project:/i)).toBeInTheDocument();
      expect(screen.getByLabelText(/Task Description:/i)).toBeInTheDocument();
      expect(screen.getByLabelText(/Date:/i)).toBeInTheDocument();
      expect(screen.getByLabelText(/Start Time:/i)).toBeInTheDocument();
      expect(screen.getByLabelText(/End Time:/i)).toBeInTheDocument();
    });

    fireEvent.change(screen.getByLabelText(/Project:/i), { target: { value: 1 } });
    fireEvent.change(screen.getByLabelText(/Task Description:/i), { target: { value: 'Test task' } });
    fireEvent.change(screen.getByLabelText(/Date:/i), { target: { value: '2022-01-01' } });
    fireEvent.change(screen.getByLabelText(/Start Time:/i), { target: { value: '09:00' } });
    fireEvent.change(screen.getByLabelText(/End Time:/i), { target: { value: '09:30' } });
    fireEvent.submit(screen.getByTestId('submit-button'));

    await waitFor(() => {
      // Assert
      expect(TimeRegistrationApi.addTimeRegistration).toHaveBeenCalledWith({
        projectId: 1,
        taskDescription: 'Test task',
        date: "2022-01-01T00:00:00.000Z",
        startTime: "2022-01-01T08:00:00.000Z",
        endTime: "2022-01-01T08:30:00.000Z",
        FreelancerId: 1,
      });
    });
  });

  it('displays errors for required fields', async () => {
    // Arrange
    const mockProjects = [
      { id: 1, name: 'Project 1', deadline: '2024-01-30', status: ProjectStatus.InProgress },
      { id: 2, name: 'Project 2', deadline: '2024-02-15', status: ProjectStatus.Complete },
    ];

    jest.spyOn(ProjectApi, 'getProjects').mockResolvedValueOnce(mockProjects);
    jest.spyOn(TimeRegistrationApi, 'addTimeRegistration').mockResolvedValueOnce({ message: 'Time registration added successfully' });

    // Act
    render(<TimeRegistrationForm onCloseForm={() => {}} onSuccessfulSubmit={() => {}} />);

    fireEvent.change(screen.getByLabelText(/Date:/i), { target: { value: '' } });

    fireEvent.submit(screen.getByTestId('submit-button'));

    await waitFor(() => {
      // Assert
      expect(screen.getByText('Project is required')).toBeInTheDocument();
      expect(screen.getByText('Task Description is required')).toBeInTheDocument();
      expect(screen.getByText('Date is required')).toBeInTheDocument();

      expect(TimeRegistrationApi.addTimeRegistration).not.toHaveBeenCalled();
    });
  });

  it('displays error for invalid time range', async () => {
    // Arrange
    const mockProjects = [
      { id: 1, name: 'Project 1', deadline: '2024-01-30', status: ProjectStatus.InProgress },
      { id: 2, name: 'Project 2', deadline: '2024-02-15', status: ProjectStatus.Complete },
    ];

    jest.spyOn(ProjectApi, 'getProjects').mockResolvedValueOnce(mockProjects);
    jest.spyOn(TimeRegistrationApi, 'addTimeRegistration').mockResolvedValueOnce({ message: 'Time registration added successfully' });
    render(<TimeRegistrationForm onCloseForm={() => {}} onSuccessfulSubmit={() => {}} />);

    // Act
    fireEvent.change(screen.getByLabelText(/Project:/i), { target: { value: 1 } });
    fireEvent.change(screen.getByLabelText(/Task Description:/i), { target: { value: 'Test task' } });
    fireEvent.change(screen.getByLabelText(/Date:/i), { target: { value: '2022-01-01' } });
    fireEvent.change(screen.getByLabelText(/Start Time:/i), { target: { value: '12:00' } });
    fireEvent.change(screen.getByLabelText(/End Time:/i), { target: { value: '11:30' } });

    fireEvent.submit(screen.getByTestId('submit-button'));

    await waitFor(() => {
      // Assert
      expect(screen.getByText('End Time must be greater than Start Time and at least 30 minutes later.')).toBeInTheDocument();
      expect(TimeRegistrationApi.addTimeRegistration).not.toHaveBeenCalled();
    });
  });

  it('displays error for End Time earlier than Start Time', async () => {
    // Arrange
    const mockProjects = [
      { id: 1, name: 'Project 1', deadline: '2024-01-30', status: ProjectStatus.InProgress },
      { id: 2, name: 'Project 2', deadline: '2024-02-15', status: ProjectStatus.Complete },
    ];

    jest.spyOn(ProjectApi, 'getProjects').mockResolvedValueOnce(mockProjects);
    jest.spyOn(TimeRegistrationApi, 'addTimeRegistration').mockResolvedValueOnce({ message: 'Time registration added successfully' });
    render(<TimeRegistrationForm onCloseForm={() => {}} onSuccessfulSubmit={() => {}} />);

    // Act
    fireEvent.change(screen.getByLabelText(/Project:/i), { target: { value: 1 } });
    fireEvent.change(screen.getByLabelText(/Task Description:/i), { target: { value: 'Test task' } });
    fireEvent.change(screen.getByLabelText(/Date:/i), { target: { value: '2022-01-01' } });
    fireEvent.change(screen.getByLabelText(/Start Time:/i), { target: { value: '12:00' } });
    fireEvent.change(screen.getByLabelText(/End Time:/i), { target: { value: '11:30' } });

    fireEvent.submit(screen.getByTestId('submit-button'));

    await waitFor(() => {
      // Assert
      expect(screen.getByText('End Time must be greater than Start Time and at least 30 minutes later.')).toBeInTheDocument();
      expect(TimeRegistrationApi.addTimeRegistration).not.toHaveBeenCalled();
    });
  });
});
