import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { FaFolderOpen } from "react-icons/fa";
import Table from "../../fragments/Table";
import Header from "../../layouts/Header";
import { Container } from "react-bootstrap";
import EmployeeStatusBadge from "./fragments/EmployeeStatusBadge";
import { useQuery } from "react-query";
import { fetchEmployees } from "../../../api/employee";
import ConnectionError from "../../fragments/ConnectionError";
import LoadingSpinner from "../../fragments/LoadingSpinner";
import { useAuth } from "../../../contexts/AuthContext";

function ListEmployees() {
    const [employees, setEmployees] = useState([]);
    const auth = useAuth();

    var { data, isLoading, isError } = useQuery("employees", fetchEmployees);

    useEffect(() => {
        if (data === undefined) return;

            Object.keys(data).map((key, index) => {
                //add status badge
                data[key].statusBadge = EmployeeStatusBadge(data[key].status);

                //add links to each row
                data[key].open = (
                    <>
                        <Link
                            className="btn btn-teal me-2"
                            to={`/employee/${data[key].employeeId}`}
                        >
                            <FaFolderOpen className="me-1 mb-1" />
                            Open
                        </Link>
                    </>
                );

                return null;
            });
            setEmployees(data);
    }, [data]);

    const columns = React.useMemo(
        () => [
            {
                Header: "Employee Id.",
                accessor: "employeeId", // accessor is the "key" in the data
            },
            {
                Header: "First Name",
                accessor: "fName", 
            },
            {
                Header: "Middle Name",
                accessor: "mName", 
            },
            {
                Header: "Last Name",
                accessor: "lName", 
            },
            {
                Header: "Status",
                accessor: "statusBadge", 
                sortType: (
                    rowA,
                    rowB //get original data of row and compare status
                ) => (rowA.original.status > rowB.original.status ? 1 : -1),
            },
            
            {
                Header: "",
                accessor: "open", 
                disableSortBy: true,
            },
        ],
        []
    );

    if (isLoading) return <LoadingSpinner />;

    if (isError) return <ConnectionError />;

    return (
        <>
            <Header title="Employees" />

            <Container className="my-3">
                <Table columns={columns} data={employees} />
            </Container>
        </>
    );
}

export default ListEmployees;
