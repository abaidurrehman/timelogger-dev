import {
    getProjectStatusString,
    formatDeadline,
    formatDateTime,
    calculateHoursWorked,
    calculateDaysUntilDeadline,
} from './helpers';
import { ProjectStatus } from './types';

describe('Utility Functions', () => {
    describe('getProjectStatusString', () => {
        it('returns correct string for ProjectStatus.New', () => {
            // Arrange & Act
            const result = getProjectStatusString(ProjectStatus.New);

            // Assert
            expect(result).toBe('New');
        });

        it('returns correct string for ProjectStatus.InProgress', () => {
            // Arrange & Act
            const result = getProjectStatusString(ProjectStatus.InProgress);

            // Assert
            expect(result).toBe('InProgress');
        });

        it('returns correct string for ProjectStatus.Complete', () => {
            // Arrange & Act
            const result = getProjectStatusString(ProjectStatus.Complete);

            // Assert
            expect(result).toBe('Complete');
        });
    });

    describe('formatDeadline', () => {
        it('formats deadline correctly', () => {
            // Arrange
            const deadline = '2024-01-30';

            // Act
            const result = formatDeadline(deadline);

            // Assert
            expect(result).toBe('January 30, 2024');
        });
    });

    describe('formatDateTime', () => {
        it('formats date and time correctly', () => {
            // Arrange
            const dateTimeString = '2024-01-30T12:30:00';

            // Act
            const result = formatDateTime(dateTimeString);

            // Assert
            expect(result).toBe('Jan 30, 2024, 12:30:00 PM');
        });
    });

    describe('calculateHoursWorked', () => {
        it('calculates hours worked correctly', () => {
            // Arrange
            const startTime = '2024-01-30T09:00:00';
            const endTime = '2024-01-30T12:30:00';

            // Act
            const result = calculateHoursWorked(startTime, endTime);

            // Assert
            expect(result).toBe('3.50');
        });
    });

    describe('calculateDaysUntilDeadline', () => {
        it('calculates days until deadline correctly', () => {
            // Arrange
            const deadline = '2024-02-15';

            // Act
            const result = calculateDaysUntilDeadline(deadline);

            // Assert
            expect(result).toBeGreaterThan(0); // Check if the result is a positive number
        });
    });
});
