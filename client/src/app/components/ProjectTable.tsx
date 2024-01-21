import React, { useEffect, useState } from "react";
import Switch from "react-switch";
import { ProjectApi } from "../api/project-api";
import { Project, ProjectStatus } from "../shared/types";
import { calculateDaysUntilDeadline, formatDeadline, getProjectStatusString } from "../shared/helpers";
import ProjectTimeRegistration from "./ProjectTimeRegistration";

const ProjectTable: React.FC = () => {
    const [projects, setProjects] = useState<Project[]>([]);
    const [selectedProjectId, setSelectedProjectId] = useState<number | null>(null);
    const [sortBy, setSortBy] = useState<string>('asc');
    const [showCompleteProjects, setShowCompleteProjects] = useState(false);

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
        return [...filteredProjects].sort((a, b) => {
            const dateA = new Date(a.deadline).getTime();
            const dateB = new Date(b.deadline).getTime();

            if (sortBy === 'asc') {
                return dateA - dateB;
            } else if (sortBy === 'desc') {
                return dateB - dateA;
            }

            return 0; // No sorting
        });
    };

    const handleSortChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const selectedSortBy = event.target.value;
        setSortBy(selectedSortBy);
    };

    const handleRowClick = (projectId: number) => {
        setSelectedProjectId(projectId);
    };

    const toggleShowCompleteProjects = () => {
        setShowCompleteProjects((prevShowCompleteProjects) => !prevShowCompleteProjects);
    };

    const filteredProjects = projects.filter((project) => {
        if (showCompleteProjects) {
            return true; // Show all projects
        } else {
            return project.status !== ProjectStatus.Complete;
        }
    });

    return (
        <div>
            <h2>Projects</h2>
            <div className="mb-4">
                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="sortDropdown">
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
                <label className="block text-gray-700 text-sm font-bold mb-2">
                    Show Complete Projects:
                </label>
                <Switch
                    checked={showCompleteProjects}
                    onChange={toggleShowCompleteProjects}
                    onColor="#86d3ff"
                    onHandleColor="#2693e6"
                    handleDiameter={20}
                    uncheckedIcon={false}
                    checkedIcon={false}
                    height={15}
                    width={40}
                />
            </div>
            <table className="table-fixed w-full">
                <thead className="bg-gray-200">
                    <tr>
                        <th className="border px-4 py-2 w-12">#</th>
                        <th className="border px-4 py-2">Project Name</th>
                        <th className="border px-4 py-2">Deadline</th>
                        <th className="border px-4 py-2">Days until Deadline</th>
                        <th className="border px-4 py-2">Status</th>
                        <th className="border px-4 py-2">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {sortProjects(sortBy).map((project) => (
                        <tr
                            key={project.id}
                            style={{ backgroundColor: getDeadlineBackgroundColor(project.deadline), cursor:"pointer" }}
                            onClick={() => handleRowClick(project.id)}
                        >
                            <td className="border px-4 py-2 w-12">{project.id}</td>
                            <td className="border px-4 py-2">{project.name}</td>
                            <td className="border px-4 py-2">{formatDeadline(project.deadline)}</td>
                            <td className="border px-4 py-2">{calculateDaysUntilDeadline(project.deadline)}</td>
                            <td className="border px-4 py-2">{getProjectStatusString(project.status)}</td>
                            <td className="border px-4 py-2">
                                <button onClick={() => handleRowClick(project.id)} className="btn btn-primary">View Time Registrations</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
            {selectedProjectId && (
                <ProjectTimeRegistration projectId={selectedProjectId} />
            )}
        </div>
    );
};

export default ProjectTable;
