import React from "react";
import { render, screen } from "@testing-library/react";
import Projects from "./Projects";
import '@testing-library/jest-dom';

describe("Projects component", () => {
    test("renders Projects component correctly", () => {
        render(<Projects />);
        const projectsComponent = screen.getByText("Projects");
        expect(projectsComponent).toBeInTheDocument();
    });
});
