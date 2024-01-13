import React, { useState, useEffect } from "react";
import { TimeRegistration } from "../shared/types";
import { TimeRegistrationApi } from "../api/time-registration-api";
import { formatDateTime, calculateHoursWorked } from "../shared/helpers";

interface ProjectTimeRegistrationProps {
    projectId: number;
}

const ProjectTimeRegistration: React.FC<ProjectTimeRegistrationProps> = ({ projectId }) => {
    const [timeRegistrations, setTimeRegistrations] = useState<TimeRegistration[]>([]);

    useEffect(() => {
        const fetchTimeRegistrations = async () => {
            try {
                const times = await TimeRegistrationApi.getTimesForProject(projectId);
                setTimeRegistrations(times);
            } catch (error: any) {
                console.error('Error fetching time registrations:', error.message);
            }
        };

        fetchTimeRegistrations();
    }, [projectId]);

    return (
        <div>
            <label className="block text-gray-700 text-sm font-bold mb-2">Time Registrations for Project {projectId}</label>
            <table className="table-fixed w-full">
                <thead className="bg-gray-200">
                    <tr>
                        <th className="border px-4 py-2">Task Description</th>
                        <th className="border px-4 py-2">Start Time</th>
                        <th className="border px-4 py-2">End Time</th>
                        <th className="border px-4 py-2">Hours Worked</th>
                    </tr>
                </thead>
                <tbody>
                    {timeRegistrations.map((timeRegistration) => (
                        <tr key={timeRegistration.id}>
                            <td className="border px-4 py-2">{timeRegistration.taskDescription}</td>
                            <td className="border px-4 py-2">{formatDateTime(timeRegistration.startTime)}</td>
                            <td className="border px-4 py-2">{formatDateTime(timeRegistration.endTime)}</td>
                            <td className="border px-4 py-2">{calculateHoursWorked(timeRegistration.startTime, timeRegistration.endTime)}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default ProjectTimeRegistration;
