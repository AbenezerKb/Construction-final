import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { FaFolderOpen } from "react-icons/fa";
import Table from "../fragments/Table";
import Header from "../layouts/Header";
import { Container } from "react-bootstrap";
import { useQuery } from "react-query";
import { fetchTransfers } from "../../api/transfer";
import { fetchAllNotifications } from "../../api/notification";
import ConnectionError from "../fragments/ConnectionError";
import LoadingSpinner from "../fragments/LoadingSpinner";
import { useAuth } from "../../contexts/AuthContext";
import { NOTIFICATIONROUTE } from "../../Constants";

function AdminNotification() {
    const [transfers, setTransfers] = useState([]);
    const auth = useAuth();

    var { data, isLoading, isError, error } = useQuery(
        ["adminNotifications"],
        fetchAllNotifications
    );

    useEffect(() => {
        if (data === undefined) return;

        Object.keys(data).map((key, index) => {
            //format date
            data[key].date = new Date(data[key].date).toLocaleString();

            data[key].name = String(NOTIFICATIONROUTE[data[key].type]).substring(1);

            //add links to each row
            data[key].open = (
                <>
                    <Link
                        className="btn btn-teal me-2"
                        to={`${NOTIFICATIONROUTE[data[key].type]}/${data[key].actionId}`}
                    >
                        <FaFolderOpen className="me-1 mb-1" />
                        Open
                    </Link>
                </>
            );

            return null;
        });

        setTransfers(data);
    }, [data]);

    const columns = React.useMemo(
        () => [
            {
                Header: "Created On",
                accessor: "date",
            },
            {
                Header: "Title",
                accessor: "title", // accessor is the "key" in the data
            },
            {
                Header: "Content",
                accessor: "content", // accessor is the "key" in the data
            },
            {
                Header: "Site",
                accessor: "site.name",
            },
            {
                Header: "Type",
                accessor: "name",
            },

            {
                Header: "",
                accessor: "open", // accessor is the "key" in the data
                disableSortBy: true,
            },
        ],
        []
    );
    if (isError) return <ConnectionError status={error?.response?.status} />;

    if (isLoading) return <LoadingSpinner />;


    return (
        <>
            <Header title="System Notifications" />

            <Container className="my-3">
                <Table columns={columns} data={transfers} />
            </Container>
        </>
    );
}

export default AdminNotification;
