import React, { Component } from "react";
import { BarChart, Bar, XAxis, YAxis, Cell, CartesianGrid } from "recharts";
import "../css/Statistics.css";

export class Statistics extends Component {
	static displayName = Statistics.name;

	state = {
		dataTally: null,
		dataCurrentCandidate: null
	};

	componentDidMount() {
		(async () => {
			const responseTally = await fetch("api/statistics/tallylines/", {
				method: "GET",
				headers: {
					"Content-Type": "application/json",
					Authorization: this.props.id
				}
			});
			const responseCurrentCandidate = await fetch(
				"api/statistics/shortestsolutions/",
				{
					method: "GET",
					headers: {
						"Content-Type": "application/json",
						Authorization: this.props.id
					}
				}
			);
			this.setState({
				dataTally: await responseTally.json(),
				dataCurrentCandidate: await responseCurrentCandidate.json()
			});
		})();
	}

	render() {
		if (this.state.dataTally) {
			const components = [];
			for (const [levelNumber, results] of Object.entries(
				this.state.dataTally
			)) {
				components.push(
					<div
						className="level-chart-container"
						key={"C" + levelNumber}
					>
						<label
							className="level-chart-label"
							key={"L" + levelNumber}
						>
							Level {levelNumber}
						</label>
						<LevelBarChart
							className="level-chart"
							key={"D" + levelNumber}
							dataTally={results}
							dataCandidate={
								this.state.dataCurrentCandidate[levelNumber]
							}
						/>
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
	const isVert = axisType === "yAxis";
	const cx = isVert ? x : x + width / 2;
	const cy = isVert ? height / 2 + y : y + height + 10;
	const rot = isVert ? `270 ${cx} ${cy}` : 0;
	return (
		<text
			x={cx}
			y={cy}
			transform={`rotate(${rot})`}
			textAnchor="middle"
			stroke={stroke}
		>
			{children}
		</text>
	);
};

class LevelBarChart extends Component {
	static displayName = LevelBarChart.name;
	constructor(props) {
		super(props);
		const _dataTally = [];
		for (const [_lines, _candidates] of Object.entries(props.dataTally)) {
			_dataTally.push({
				lines: _lines,
				candidates: Number.parseInt(_candidates)
			});
		}
		this.state = { dataTally: _dataTally };
	}

	render() {
		return (
			<BarChart
				width={300}
				height={200}
				data={this.state.dataTally}
				margin={50}
			>
				<CartesianGrid strokeDasharray="3 3" />
				<XAxis
					dataKey="lines"
					label={{ value: "Lines of Code", position: "insideBottom" }}
					height={40}
				/>
				<YAxis
					label={
						<AxisLabel
							axisType="yAxis"
							x={25}
							y={-75}
							width={0}
							height={300}
						>
							Candidates
						</AxisLabel>
					}
					allowDecimals={false}
				/>
				<Bar dataKey="candidates">
					{this.state.dataTally.map((entry, index) => {
						return (
							<Cell
								key={`cell-${index}`}
								fill={
									entry.lines === this.props.dataCandidate
										? "#00AA00"
										: "#8CA183"
								}
							/>
						);
					})}
				</Bar>
			</BarChart>
		);
	}
}
