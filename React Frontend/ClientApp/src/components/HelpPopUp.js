import React, { useState } from "react";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";

export const HelpPopUp = props => {
	const { buttonLabel } = props;

	const [modal, setModal] = useState(false);

	const toggle = () => setModal(!modal);

	return (
		<div>
			<Button color="info" onClick={toggle}>
				{buttonLabel}
			</Button>
			<Modal isOpen={modal} toggle={toggle} className="modal-xl">
				<ModalHeader toggle={toggle}>Tutorial</ModalHeader>
				<ModalBody>
					Klik op commandos en statements om de character bij de schatkist te laten komen. 
					[GIF van character die op schatkist loopt]

					Commandos en statements klikken in elkaar om zo het karakter uit te laten voeren wat jij hebt geprogrammeerd.
					[GIF van statement die wordt geselecteerd en in een conditie oid wordt geklikt]

					Druk op RUN om jou ingevoerde programma uit te voeren!
				</ModalBody>
				<ModalFooter>
					<Button color="primary" onClick={toggle}>
						Close
					</Button>
				</ModalFooter>
			</Modal>
		</div>
	);
};
