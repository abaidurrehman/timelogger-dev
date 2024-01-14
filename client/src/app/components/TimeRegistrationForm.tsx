import React, { useState, useEffect } from 'react';
import { ProjectApi } from "../api/project-api";
import { Project, TimeRegistration } from "../shared/types";
import { TimeRegistrationApi } from '../api/time-registration-api';

interface TimeRegistrationFormProps {
    onCloseForm: () => void;
}

const TimeRegistrationForm: React.FC<TimeRegistrationFormProps> = ({ onCloseForm }) => {
    const [formData, setFormData] = useState<TimeRegistration>({
        projectId: 0,
        taskDescription: '',
        date: new Date().toISOString().split('T')[0], // Default to today's date
        startTime: '09:00',
        endTime: '09:30',
        FreelancerId: 0
    });

    const [projects, setProjects] = useState<Project[]>([]);
    const [error, setError] = useState<{ [key: string]: string }>({});

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await ProjectApi.getProjects();
                setProjects(data);
            } catch (error: any) {
                console.error('Error fetching data:', error.message);
            }
        };

        fetchData();
    }, []);

    const validateForm = () => {
        const errors: { [key: string]: string } = {};

        if (formData.projectId === 0) {
            errors.projectId = 'Project is required';
        }

        if (!formData.taskDescription) {
            errors.taskDescription = 'Task Description is required';
        }

        if (!formData.date) {
            errors.date = 'Date is required';
        }

        if (!formData.startTime) {
            errors.startTime = 'Start Time is required';
        }

        if (!formData.endTime) {
            errors.endTime = 'End Time is required';
        }

        const startDateTime = new Date(`${formData.date}T${formData.startTime}`);
        const endDateTime = new Date(`${formData.date}T${formData.endTime}`);

        const minimumEndTime = new Date(startDateTime.getTime() + 30 * 60000);

        if (endDateTime <= startDateTime || endDateTime < minimumEndTime) {
            errors.endTime = 'End Time must be greater than Start Time and at least 30 minutes later.';
        }

        setError(errors);

        return Object.keys(errors).length === 0;
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!validateForm()) {
            return;
        }

        const combinedStartTime = new Date(`${formData.date}T${formData.startTime}`);
        const combinedEndTime = new Date(`${formData.date}T${formData.endTime}`);

        try {
            const response = await TimeRegistrationApi.addTimeRegistration({
                ...formData,
                date: new Date(formData.date).toISOString(),
                startTime: combinedStartTime.toISOString(),
                endTime: combinedEndTime.toISOString(),
                FreelancerId: 1
            });
            console.log(response.message);

            // Clear the form and reset error state after successful submission
            setFormData({
                projectId: 0,
                taskDescription: '',
                date: new Date().toISOString().split('T')[0],
                startTime: '09:00',
                endTime: '09:30',
                FreelancerId: 0
            });
            setError({});
        } catch (err: any) {
            setError({ submit: err.message });
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        const updatedValue = name === 'projectId' ? parseInt(value, 10) : value;

        setFormData({
            ...formData,
            [name]: updatedValue,
        });
    };

    const generateTimeOptions = () => {
        const options = [];
        for (let i = 0; i < 24 * 6; i++) { // Using 10-minute intervals, so 6 intervals per hour
            const hour = Math.floor(i / 6);
            const minute = (i % 6) * 10; // 10-minute intervals
            const time = `${hour.toString().padStart(2, '0')}:${minute.toString().padStart(2, '0')}`;
            options.push(time);
        }
        return options;
    };

    return (
        <form onSubmit={handleSubmit} className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
            <div className="mb-4">
                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="projectId">
                    Project:
                </label>
                <select
                    name="projectId"
                    value={formData.projectId}
                    onChange={handleChange}
                    className="w-full p-2 border rounded"
                >
                    <option value={0}>Select Project</option>
                    {projects.map(project => (
                        <option key={project.id} value={project.id}>
                            {project.name}
                        </option>
                    ))}
                </select>
                {error.projectId && <p style={{ color: 'red' }}>{error.projectId}</p>}
            </div>

            <div className="mb-4">
                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="taskDescription">
                    Task Description:
                </label>
                <input
                    type="text"
                    name="taskDescription"
                    value={formData.taskDescription}
                    onChange={handleChange}
                    className="w-full p-2 border rounded"
                />
                {error.taskDescription && <p style={{ color: 'red' }}>{error.taskDescription}</p>}
            </div>

            <div className="mb-4">
                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="date">
                    Date:
                </label>
                <input
                    type="date"
                    name="date"
                    value={formData.date}
                    onChange={handleChange}
                    className="w-full p-2 border rounded"
                />
                {error.date && <p style={{ color: 'red' }}>{error.date}</p>}
            </div>

            <div className="mb-4">
                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="startTime">
                    Start Time:
                </label>
                <select
                    name="startTime"
                    value={formData.startTime}
                    onChange={handleChange}
                    className="w-full p-2 border rounded"
                >
                    {generateTimeOptions().map(time => (
                        <option key={time} value={time}>
                            {time}
                        </option>
                    ))}
                </select>
                {error.startTime && <p style={{ color: 'red' }}>{error.startTime}</p>}
            </div>

            <div className="mb-4">
                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="endTime">
                    End Time:
                </label>
                <select
                    name="endTime"
                    value={formData.endTime}
                    onChange={handleChange}
                    className="w-full p-2 border rounded"
                >
                    {generateTimeOptions().map(time => (
                        <option key={time} value={time}>
                            {time}
                        </option>
                    ))}
                </select>
                {error.endTime && <p style={{ color: 'red' }}>{error.endTime}</p>}
            </div>

            {error.submit && <p style={{ color: 'red' }}>{error.submit}</p>}

            <div className="flex items-center justify-between">
                <button type="submit" className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
                    Submit
                </button>
                <button
                    type="button"
                    onClick={onCloseForm}
                    className="bg-gray-500 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded"
                >
                    Cancel
                </button>
            </div>
        </form>
    );
};

export default TimeRegistrationForm;
