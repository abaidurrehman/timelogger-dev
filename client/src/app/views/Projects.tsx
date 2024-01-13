import React, { useState } from "react";
import ProjectTable from "../components/ProjectTable";
import TimeRegistrationForm from "../components/TimeRegistrationForm";

export default function Projects() {
    const [showTimeRegistrationForm, setShowTimeRegistrationForm] = useState(false);

    const handleAddEntryClick = () => {
        setShowTimeRegistrationForm(true);
    };

    const handleCloseForm = () => {
        setShowTimeRegistrationForm(false);
    };

    return (
        <>
            <div className="flex items-center my-6">
                <div className="w-1/2">
                    <button
                        className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
                        onClick={handleAddEntryClick}
                    >
                        Register time
                    </button>
                </div>

                <div className="w-1/2 flex justify-end">
                    <form>
                        <input
                            className="border rounded-full py-2 px-4"
                            type="search"
                            placeholder="Search"
                            aria-label="Search"
                        />
                        <button
                            className="bg-blue-500 hover:bg-blue-700 text-white rounded-full py-2 px-4 ml-2"
                            type="submit"
                        >
                            Search
                        </button>
                    </form>
                </div>
            </div>
            {showTimeRegistrationForm && (
                <div className="my-6">
                    <TimeRegistrationForm onCloseForm={handleCloseForm} />
                </div>
            )}
            <ProjectTable />
        </>
    );
}
