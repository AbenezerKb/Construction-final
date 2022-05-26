import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { FaFolderOpen } from "react-icons/fa";
import Table from "../../fragments/Table";
import Header from "../../layouts/Header";
import { Container } from "react-bootstrap";
import { useQuery } from "react-query";
import { fetchReturns } from "../../../api/return";
import ConnectionError from "../../fragments/ConnectionError";
import LoadingSpinner from "../../fragments/LoadingSpinner";

function ListReturns() {
    const [maintenances, setMaintenances] = useState([]);

    var { data, isLoading, isError, error } = useQuery("returns", fetchReturns);

    useEffect(() => {
        if (data === undefined) return;

        Object.keys(data).map((key, index) => {
            //format date
            data[key].returnDate = new Date(data[key].returnDate).toLocaleString();

            //add links to each row
            data[key].open = (
                <>
                    <Link
                        className="btn btn-teal me-2"
                        to={`/return/${data[key].returnId}`}
                    >
                        <FaFolderOpen className="me-1 mb-1" />
                        Open
                    </Link>
                </>
            );

            return null;
        });

        setMaintenances(data);
    }, [data]);

    const columns = React.useMemo(
        () => [
            {
                Header: "Return No.",
                accessor: "returnId", // accessor is the "key" in the data
            },
            {
                Header: "Returned On",
                accessor: "returnDate",
            },
            {
                Header: "Site",
                accessor: "site.name",
            },
            {
                Header: "",
                accessor: "open", // accessor is the "key" in the data
                disableSortBy: true,
            },
        ],
        []
    );

    if (isLoading) return <LoadingSpinner />;

    if (isError) return <ConnectionError status={error?.response?.status}/>;

    return (
        <>
            <Header title="Issues" />

            <Container className="my-3">
                <Table columns={columns} data={maintenances} />
            </Container>
        </>
    );
}

export default ListReturns;
