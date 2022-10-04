import React from 'react';
import Card from 'react-bootstrap/Card';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { propTypes } from 'react-bootstrap/esm/Image';
import './license.css';
import AddLicensesForm from './AddLicensesForm';

function LicensesUserPage() {
    return (
        <Container className="form-container-license" key={propTypes.Id}>
            <Container>
                <Row className="card card-form-row-license">
                    <Col>
                        <Card.Header className="card-header-form-license"></Card.Header>
                        <AddLicensesForm></AddLicensesForm>
                    </Col>
                </Row>
            </Container>
        </Container>
    );
}

export default LicensesUserPage;
