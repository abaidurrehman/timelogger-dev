import { TimeRegistrationApi } from './time-registration-api';
import { HttpHelper } from './http-helper';
import { TimeRegistration } from '../shared/types';

const timeRegistrationMock: TimeRegistration = {
  projectId: 1,
  FreelancerId: 123,
  taskDescription: 'Task description goes here',
  date: '2024-01-18',
  startTime: '09:00 AM',
  endTime: '05:00 PM',
};

jest.mock('./http-helper', () => ({
  HttpHelper: {
    makeApiRequest: jest.fn(),
  },
}));

describe('TimeRegistrationApi', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  describe('addTimeRegistration', () => {
    it('should make a POST request to the correct endpoint with the provided data', async () => {
      // Arrange
      const expectedEndpoint = '/timeregistration';

      // Act
      await TimeRegistrationApi.addTimeRegistration(timeRegistrationMock);

      // Assert
      expect(HttpHelper.makeApiRequest).toHaveBeenCalledWith(
        expectedEndpoint,
        'POST',
        timeRegistrationMock
      );
    });
  });

  describe('getTimesForProject', () => {
    it('should make a GET request to the correct endpoint with the provided projectId', async () => {
      // Arrange
      const projectId = 123;
      const expectedEndpoint = `/timeregistration/GetTimesForProject/${projectId}`;

      // Act
      await TimeRegistrationApi.getTimesForProject(projectId);

      // Assert
      expect(HttpHelper.makeApiRequest).toHaveBeenCalledWith(
        expectedEndpoint,
        'GET'
      );
    });
  });

  describe('checkDuplicateTime', () => {
    it('should return true if response message contains "duplicate"', async () => {
      // Arrange
      const mockResponse = { message: 'Duplicate time entry' };
      (HttpHelper.makeApiRequest as jest.Mock).mockResolvedValue(mockResponse);

      // Act
      const result = await TimeRegistrationApi.checkDuplicateTime(timeRegistrationMock);

      // Assert
      expect(result).toBe(true);
    });
  });
});
