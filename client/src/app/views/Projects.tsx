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

    const handleSuccessfulSubmit = () => {
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
                            <TimeRegistrationForm onCloseForm={handleCloseForm} onSuccessfulSubmit={handleSuccessfulSubmit} />
                        </div>
                    </div>
                </div>
            )}
            <ProjectTable />
        </>
    );
}
