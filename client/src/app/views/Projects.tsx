import React from "react";
import ProjectTable from "../components/ProjectTable";

export default function Projects() {

    return (
        <>
            <div className="flex items-center my-6">
                <div className="w-1/2">
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
            <ProjectTable />
        </>
    );
}
