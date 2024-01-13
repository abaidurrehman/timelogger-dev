import React, { useEffect, useState } from "react";
import projectApi from "../api/projects";
import { Project } from "../shared/types";
import { formatDeadline, getProjectStatusString } from "../shared/helpers";

const ProjectTable: React.FC = () => {
    const [projects, setProjects] = useState<Project[]>([]);
    const [sortBy, setSortBy] = useState<string>('asc');

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await projectApi.getProjects();
                setProjects(data);
            } catch (error: any) {
                console.error('Error fetching data:', error.message);
            }
        };

        fetchData();
    }, []); // Empty dependency array means this effect runs once when the component mounts

    const calculateDaysUntilDeadline = (deadline: string): number => {
        const today = new Date();
        const deadlineDate = new Date(deadline);
        const timeDifference = deadlineDate.getTime() - today.getTime();
        return Math.ceil(timeDifference / (1000 * 3600 * 24)); // Convert milliseconds to days
    };

    const getDeadlineBackgroundColor = (deadline: string): string => {
        const daysUntilDeadline = calculateDaysUntilDeadline(deadline);
        if (daysUntilDeadline <= 2) {
            return '#FFCCCC'; // Light red for deadlines within 2 days
        } else if (daysUntilDeadline <= 10) {
            return '#FFE4B5'; // Moccasin for deadlines within 7 days
        }
        return 'transparent'; // Default color for deadlines further away
    };

    const sortProjects = (sortBy: string) => {
        const sortedProjects = [...projects].sort((a, b) => {
            const dateA = new Date(a.deadline).getTime();
            const dateB = new Date(b.deadline).getTime();

            if (sortBy === 'asc') {
                return dateA - dateB;
            } else if (sortBy === 'desc') {
                return dateB - dateA;
            }

            return 0; // No sorting
        });

        return sortedProjects;
    };

    const handleSortChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const selectedSortBy = event.target.value;
        setSortBy(selectedSortBy);
    };

    return (
        <div>
            <h2>Projects</h2>
            <div className="mb-4">
                <label className="block text-gray-700 text-sm font-bold mb-2">
                    Sort by Deadline:
                </label>
                <select
                    value={sortBy}
                    onChange={handleSortChange}
                    className="w-full p-2 border rounded max-w-xs"
                >
                    <option value="">Select</option>
                    <option value="asc">Ascending</option>
                    <option value="desc">Descending</option>
                </select>
            </div>
            <table className="table-fixed w-full">
                <thead className="bg-gray-200">
                    <tr>
                        <th className="border px-4 py-2 w-12">#</th>
                        <th className="border px-4 py-2">Project Name</th>
                        <th className="border px-4 py-2">Deadline</th>
                        <th className="border px-4 py-2">Status</th>
                    </tr>
                </thead>
                <tbody>
                    {sortProjects(sortBy).map((project) => (
                        <tr
                            key={project.id}
                            style={{ backgroundColor: getDeadlineBackgroundColor(project.deadline) }}
                        >
                            <td className="border px-4 py-2 w-12">{project.id}</td>
                            <td className="border px-4 py-2">{project.name}</td>
                            <td className="border px-4 py-2">{formatDeadline(project.deadline)}</td>
                            <td className="border px-4 py-2">{getProjectStatusString(project.status)}</td>
                        </tr>
                    ))}

                </tbody>
            </table>
        </div>
    );
};

export default ProjectTable;