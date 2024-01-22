import React, { useState, useEffect } from 'react';
import { ProjectApi } from "../api/project-api";
import { Project, TimeRegistration } from "../shared/types";
import { TimeRegistrationApi } from '../api/time-registration-api';

interface TimeRegistrationFormProps {
    onCloseForm: () => void;
    onSuccessfulSubmit: () => void;
    defaultProjectId?: number;
}

const ErrorDisplay: React.FC<{ error?: string }> = ({ error }) => (
    <p style={{ color: 'red' }}>{error}</p>
);

const TimeRegistrationForm: React.FC<TimeRegistrationFormProps> = ({ onCloseForm, onSuccessfulSubmit,defaultProjectId }) => {
    const initialFormData = {
        projectId: defaultProjectId || 0,
        taskDescription: '',
        date: new Date().toISOString().split('T')[0],
        startTime: '09:00',
        endTime: '09:30',
        FreelancerId: 0
    };

    const [formData, setFormData] = useState<TimeRegistration>(initialFormData);
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

        if (new Date(formData.startTime) > new Date()) {
            errors.startTime = 'Start time cannot be in the future.';
        }

        if (new Date(formData.endTime) > new Date()) {
            errors.endTime = 'End time cannot be in the future.';
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

            if (!response.errors) {
                setFormData(initialFormData);
                onSuccessfulSubmit();
            } else {
                setError((prevError) => ({
                    ...prevError,
                    submit: response?.errors?.join(', ') ?? '',
                }));
            }
        } catch (err: any) {
            console.error(err.message);
            setError({ submit: err.errors?.join(', ') || '' });
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
        for (let i = 0; i < 24 * 6; i++) {
            const hour = Math.floor(i / 6);
            const minute = (i % 6) * 10;
            const time = `${hour.toString().padStart(2, '0')}:${minute.toString().padStart(2, '0')}`;
            options.push(time);
        }
        return options;
    };

    const inputStyles = `w-full p-2 border rounded ${error.projectId ? 'border-red-500' : ''}`;

    const renderTimeOptions = () => generateTimeOptions().map(time => (
        <option key={time} value={time}>
            {time}
        </option>
    ));

    return (
        <form onSubmit={handleSubmit} className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
            <div className="mb-4">
                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="projectId">
                    Project:
                </label>
                <select
                    id="projectId"
                    name="projectId"
                    value={formData.projectId}
                    onChange={handleChange}
                    className={inputStyles}
                >
                    <option value={0}>Select Project</option>
                    {projects.map(project => (
                        <option key={project.id} value={project.id}>
                            {project.name}
                        </option>
                    ))}
                </select>
                <ErrorDisplay error={error.projectId} />
            </div>

            <div className="mb-4">
                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="taskDescription">
                    Task Description:
                </label>
                <input
                    type="text"
                    name="taskDescription"
                    id="taskDescription"
                    value={formData.taskDescription}
                    onChange={handleChange}
                    className={`w-full p-2 border rounded ${error.taskDescription ? 'border-red-500' : ''}`}
                />
                <ErrorDisplay error={error.taskDescription} />
            </div>

            <div className="mb-4">
                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="date">
                    Date:
                </label>
                <input
                    type="date"
                    name="date"
                    id="date"
                    value={formData.date}
                    onChange={handleChange}
                    className={`w-full p-2 border rounded ${error.date ? 'border-red-500' : ''}`}
                />
                <ErrorDisplay error={error.date} />
            </div>

            <div className="mb-4">
                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="startTime">
                    Start Time:
                </label>
                <select
                    name="startTime"
                    id="startTime"
                    value={formData.startTime}
                    onChange={handleChange}
                    className={inputStyles}
                >
                    {renderTimeOptions()}
                </select>
                <ErrorDisplay error={error.startTime} />
            </div>

            <div className="mb-4">
                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="endTime">
                    End Time:
                </label>
                <select
                    name="endTime"
                    id="endTime"
                    value={formData.endTime}
                    onChange={handleChange}
                    className={inputStyles}
                >
                    {renderTimeOptions()}
                </select>
                <ErrorDisplay error={error.endTime} />
            </div>

            {error.submit && <ErrorDisplay error={error.submit} />}

            <div className="flex items-center justify-between">
                <button type="submit" name="submit" data-testid="submit-button" className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
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
