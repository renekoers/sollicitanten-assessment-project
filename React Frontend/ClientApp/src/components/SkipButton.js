import React, {
    Component
} from "react";
import '../styles/SkipButton.css'

export class SkipButton extends Component {
    constructor(props) {
        super(props)
        this.skipButtonClicked = this.skipButtonClicked.bind(this);
    }
    skipButtonClicked() {
        this.props.onClick();
    }
    render() {
        let classNames;
        if (this.props.disabled) {
            classNames = ["skipButton", "disabled"].join(' ');
        } else {
            classNames = "skipButton";
        }
        return ( <
            button disabled = {
                this.props.disabled
            }
            className = {
                classNames
            }
            onClick = {
                this.skipButtonClicked
            } > {
                this.props.name
            }
            level < /button>
        );
    }
}