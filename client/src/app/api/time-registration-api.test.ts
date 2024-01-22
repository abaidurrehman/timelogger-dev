import axios from 'axios';
import { TimeRegistrationApi } from './time-registration-api';
import { TimeRegistration } from '../shared/types';

const timeRegistrationMock: TimeRegistration = {
  projectId: 1,
  FreelancerId: 123,
  taskDescription: 'Task description goes here',
  date: '2024-01-18',
  startTime: '09:00 AM',
  endTime: '05:00 PM',
};

jest.mock('axios');

describe('TimeRegistrationApi', () => {
  describe('addTimeRegistration', () => {
    it('should make a POST request to the correct endpoint with the provided data', async () => {
      // Arrange
      const expectedEndpoint = '/timeregistration';
      (axios.post as jest.Mock).mockResolvedValue({ data: {} });

      // Act
      await TimeRegistrationApi.addTimeRegistration(timeRegistrationMock);

      // Assert
      expect(axios.post).toHaveBeenCalledWith(
        expect.stringContaining(expectedEndpoint),
        timeRegistrationMock,
        expect.any(Object)
      );
    });
  });

  describe('getTimesForProject', () => {
    it('should make a GET request to the correct endpoint with the provided projectId', async () => {
      // Arrange
      const projectId = 123;
      const expectedEndpoint = `/timeregistration/GetTimesForProject/${projectId}`;
      (axios.get as jest.Mock).mockResolvedValue({ data: {} });

      // Act
      await TimeRegistrationApi.getTimesForProject(projectId);

      // Assert
      expect(axios.get).toHaveBeenCalledWith(
        expect.stringContaining(expectedEndpoint),
        expect.any(Object)
      );
    });
  });

  describe('checkDuplicateTime', () => {
    it('should return true if response message contains "duplicate"', async () => {
      // Arrange
      const mockResponse = { data: { message: 'Duplicate time entry' } };
      (axios.post as jest.Mock).mockResolvedValue(mockResponse);

      // Act
      const result = await TimeRegistrationApi.checkDuplicateTime(timeRegistrationMock);

      // Assert
      expect(result).toBe(true);
    });
  });
});
