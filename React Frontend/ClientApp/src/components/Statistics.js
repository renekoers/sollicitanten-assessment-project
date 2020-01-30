import React, { Component } from 'react';
import {
    BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip,
} from 'recharts';

/*const data = [
    {
        name: 'Page A', uv: 4000, pv: 2400, amt: 2400,
    },
    {
        name: 'Page B', uv: 3000, pv: 1398, amt: 2210,
    },
    {
        name: 'Page C', uv: 2000, pv: 9800, amt: 2290,
    },
    {
        name: 'Page D', uv: 2780, pv: 3908, amt: 2000,
    },
    {
        name: 'Page E', uv: 1890, pv: 4800, amt: 2181,
    },
    {
        name: 'Page F', uv: 2390, pv: 3800, amt: 2500,
    },
    {
        name: 'Page G', uv: 3490, pv: 4300, amt: 2100,
    },
];*/

export class Statistics extends Component {
    static displayName = Statistics.name;
    state = {
        data: null,
    }

    componentDidMount() {
        (async () => {
            const response = await fetch("api/statistics/tallylines/0");
            this.setState({ data: await response.json() });
        })();
    }

    render() {
        if (this.state.data) {
            console.log(this.state.data);
            const components = []
            for (const [levelNumber, results] of Object.entries(this.state.data)) {
                components.push(
                    <div className="level-chart-container" key={"C" + levelNumber}>
                        <label className="chart-level-label" key={"L" + levelNumber}>Level {levelNumber}</label>
                        <LevelBarChart classNAme="level-chart" key={"D" + levelNumber} data={results} />
                    </div>
                );
            }
            return components;
        } else {
            return null;
        }
    }
}

const AxisLabel = ({ axisType, x, y, width, height, stroke, children }) => {
    const isVert = axisType === 'yAxis';
    const cx = isVert ? x : x + (width / 2);
    const cy = isVert ? (height / 2) + y : y + height + 10;
    const rot = isVert ? `270 ${cx} ${cy}` : 0;
    return (
        <text x={cx} y={cy} transform={`rotate(${rot})`} textAnchor="middle" stroke={stroke}>
            {children}
        </text>
    );
};

class LevelBarChart extends Component {
    static displayName = LevelBarChart.name;
    constructor(props) {
        super(props);
        const _data = [];
        for (const [_lines, _candidates] of Object.entries(props.data)) {
            _data.push({ lines: _lines, candidates: Number.parseInt(_candidates) });
        }
        this.state = ({ data: _data });
    }

    render() {
        return (
            <BarChart
                width={300}
                height={200}
                data={this.state.data}
                margin={50}
            >
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="lines" label={{ value: "Lines of Code", position: "insideBottom" }} height={40} />
                <YAxis
                    label={<AxisLabel
                        axisType='yAxis'
                        x={25}
                        y={-75}
                        width={0}
                        height={300}>
                        Candidates
                    </AxisLabel>}
                    allowDecimals={false}
                />
                <Tooltip />
                <Bar dataKey="candidates" fill="#008000" />
            </BarChart>
        );
    }
}