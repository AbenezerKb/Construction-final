import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
    BarElement,
    ArcElement,
} from "chart.js";
import { Container } from "react-bootstrap";
import { Bar, Line, Pie } from "react-chartjs-2";

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Legend,
    Title,
    Tooltip,
    BarElement,
    ArcElement
);

const backgroundColors = [
    "rgba(75, 192, 192, 0.6)",
    "rgba(54, 162, 235, 0.6)",
    "rgba(255, 99, 132, 0.6)",
    "rgba(255, 206, 86, 0.6)",
    "rgba(153, 102, 255, 0.6)",
    "rgba(255, 159, 64, 0.6)",
];

const borderColors = [
    "rgba(75, 192, 192, 1)",
    "rgba(54, 162, 235, 1)",
    "rgba(255, 99, 132, 1)",
    "rgba(255, 206, 86, 1)",
    "rgba(153, 102, 255, 1)",
    "rgba(255, 159, 64, 1)",
];

function Chart({ type, data }) {
    for (var i = 0; i < data.datasets.length; i++) {
        data.datasets[i] = {
            ...data.datasets[i],
            backgroundColor: backgroundColors[i],
            borderColor: borderColors[i],
            borderWidth: 1,
        };
    }

    const options = {
        scales: {
            y: {
                beginAtZero: true,
            },
        },
    };

    if (type == "line")
        return (
            <Container className="mb-4" style={{ maxHeight: "50%" }}>
                <Line data={data} options={options} />
            </Container>
        );

    return (
        <Container className="mb-4" style={{ maxHeight: "50%" }}>
            <Bar data={data} options={options} />
        </Container>
    );
}

export default Chart;
