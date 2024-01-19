import React, { useState, useEffect } from "react";
import { TimeRegistration } from "../shared/types";
import { TimeRegistrationApi } from "../api/time-registration-api";
import { formatDateTime, calculateHoursWorked } from "../shared/helpers";
import TimeRegistrationForm from "./TimeRegistrationForm";

interface ProjectTimeRegistrationProps {
    projectId: number;
}

const ProjectTimeRegistration: React.FC<ProjectTimeRegistrationProps> = ({ projectId }) => {
    const [timeRegistrations, setTimeRegistrations] = useState<TimeRegistration[]>([]);
    const [showTimeRegistrationForm, setShowTimeRegistrationForm] = useState(false);

    useEffect(() => {
        fetchTimeRegistrations();
    }, [projectId]);

    const fetchTimeRegistrations = async () => {
        try {
            const times = await TimeRegistrationApi.getTimesForProject(projectId);
            setTimeRegistrations(times);
        } catch (error: any) {
            console.error('Error fetching time registrations:', error.message);
        }
    };

    const handleAddTimeRegistrationClick = () => {
        setShowTimeRegistrationForm(true);
    };

    const handleCloseTimeRegistrationForm = () => {
        setShowTimeRegistrationForm(false);
        // Refresh time registrations after closing the form
        fetchTimeRegistrations();
    };

    return (
        <div>
            <label className="block text-gray-700 text-sm font-bold mb-2">
                Time Registrations for Project {projectId}
            </label>
            <button
                onClick={handleAddTimeRegistrationClick}
                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded mb-4"
            >
                Register Time
            </button>
             {/* Show the TimeRegistrationForm in a modal */}
             {showTimeRegistrationForm && (
                <div className="fixed z-10 inset-0 overflow-y-auto">
                    <div className="flex items-center justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
               
                        <div className="fixed inset-0 transition-opacity" aria-hidden="true">
                            <div className="absolute inset-0 bg-gray-500 opacity-75"></div>
                        </div>

                        {/* Dialog content */}
                        <span className="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">
                            &#8203;
                        </span>
                        <div
                            className="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full"
                        >
                            <TimeRegistrationForm onCloseForm={handleCloseTimeRegistrationForm} onSuccessfulSubmit={handleCloseTimeRegistrationForm} />
                        </div>
                    </div>
                </div>
            )}
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
                            <td className="border px-4 py-2">
                                {calculateHoursWorked(timeRegistration.startTime, timeRegistration.endTime)}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default ProjectTimeRegistration;
