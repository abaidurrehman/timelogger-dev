import { ProjectStatus } from "./types";

export const getProjectStatusString = (status: ProjectStatus): string => {
    switch (status) {
        case ProjectStatus.New:
            return 'New';
        case ProjectStatus.InProgress:
            return 'InProgress';
        case ProjectStatus.Complete:
            return 'Complete';
        default:
            return '';
    }
};

export const formatDeadline = (deadline: string): string => {
    return new Date(deadline).toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' });
};



export const calculateDaysUntilDeadline = (deadline: string): number => {
    const today = new Date();
    const deadlineDate = new Date(deadline);
    const timeDifference = deadlineDate.getTime() - today.getTime();
    return Math.ceil(timeDifference / (1000 * 3600 * 24)); // Convert milliseconds to days
};