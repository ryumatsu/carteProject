import React, { useState, useEffect } from 'react';
import Card from 'react-bootstrap/Card';
import Container from 'react-bootstrap/Container';
import './license.css';
import debug from 'sabio-debug';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import ToggleButton from 'react-bootstrap/ToggleButton';
import toastr from 'toastr';
import Table from 'react-bootstrap/Table';
import { FaCheck } from 'react-icons/fa';
import licenseServices from '../../services/licenseServices';
import Pagination from 'rc-pagination';
import 'rc-pagination/assets/index.css';
import Form from 'react-bootstrap/Form';

const _logger = debug.extend('LicensesAdminPage');

function LicensesTableList() {
    const [radioValue, setRadioValue] = useState('1');
    const [licenses, setLicenses] = useState([]);
    const [licensesCopy, setLicensesCopy] = useState([]);
    const [pCount, setpCount] = useState([]);
    const [currentPage, setCurrentPage] = useState(0);

    const [query, setQuery] = useState('');

    useEffect(() => {
        query.length > 2
            ? licenseServices.queryLicenseTable(currentPage, 10, query).then(onQuerySuccess).catch(onQueryError)
            : licenseServices.callLicenseTable(currentPage, 10).then(onCallBtnSuccess).catch(onCallBtnError);
        _logger('useEffect fx to call list of states for drop down menu');
    }, [currentPage, query]);

    const onQueryError = (err) => {
        _logger('query failed to connect to the database', err);
        toastr.error('Something went wrong! Query did not connect to the database.');
    };

    const onQuerySuccess = (response) => {
        const queriedLicenses = response.item.pagedItems;
        _logger('Query has connected to the database', response);

        setLicenses(queriedLicenses);
        setpCount(response.item.totalCount);
    };

    const search = (data) => {
        return data.filter((licenses) => licenses.email.toLowerCase().includes(query));
    };

    const mapLicenses = (license) => {
        return (
            <tbody key={license.licenseId.id}>
                <tr>
                    <td>{license.licenseNumber}</td>
                    <td>{license.licenseStateId.name}</td>
                    <td>{license.licenseTypeId.name}</td>
                    <td>{license.dateExpires.slice(0, 10)}</td>
                    <td>{license.isVerified && <FaCheck />}</td>
                    <td>{license.email}</td>
                </tr>
            </tbody>
        );
    };

    const allBtnClick = (e) => {
        const value = e.currentTarget.id;
        _logger('allBTNClick function has fired', e, value);
        setLicenses(licensesCopy);
    };

    const onCallBtnSuccess = (response) => {
        const licenses = response.item.pagedItems;
        const count = response.item.totalCount;
        _logger('allBTN success', licenses);
        setLicenses(licenses);
        setLicensesCopy(licenses);
        setpCount(count);
    };

    const radioValues = (e) => {
        setRadioValue(e.currentTarget.value);
    };

    const onCallBtnError = (err) => {
        _logger('allBTN failed to connect to the database', err);
    };

    const verifyBtnClicked = (e) => {
        const { id, value } = e.target;

        _logger(id, value);

        var isTrue = false;
        if (value === '1') {
            isTrue = true;
        }

        const filteredStates = licensesCopy.filter((verifyLicense) => verifyLicense.isVerified === isTrue);
        setLicenses(filteredStates);
    };

    const buttons = [{ name: 'All', value: true }];

    const updatePage = (p) => {
        setCurrentPage(p);
    };

    _logger('Nav tab');
    return (
        <Container>
            <Card>
                <Card.Header className="card-header-form">
                    <Card.Title>
                        <h1>License Search</h1>
                    </Card.Title>
                </Card.Header>
                <Card.Title>
                    <ButtonGroup className="mb-2">
                        <ToggleButton
                            key={buttons.idx}
                            id={buttons.idx}
                            type="radio"
                            variant="info"
                            name="All"
                            value={false}
                            checked={radioValue === buttons.value}
                            onChange={radioValues}
                            onClick={allBtnClick}>
                            All
                        </ToggleButton>
                        &nbsp;
                        <select name="licenseState" id="licenseState" onChange={verifyBtnClicked}>
                            <option>Verified</option>
                            <option value={1}>Yes</option>
                            <option value={0}>No</option>
                        </select>
                        &nbsp;
                        <Form.Group className="createdby-form-license-admin">
                            <Form.Control
                                type="text"
                                placeholder="Enter search term"
                                onChange={(e) => setQuery(e.target.value)}
                                className="search"
                                input="text"
                            />
                        </Form.Group>
                    </ButtonGroup>
                </Card.Title>
                <Card.Body onChange={(e) => setQuery(e.target.value)}>
                    <Table striped bordered variant="dark" data={search(licenses)}>
                        <thead>
                            <tr>
                                <th>License Number</th>
                                <th>State</th>
                                <th>type</th>
                                <th>Expiration Date</th>
                                <th>Verified</th>
                                <th>Created By</th>
                            </tr>
                        </thead>
                        {licenses && licenses.map(mapLicenses)}
                    </Table>
                </Card.Body>
                <Pagination
                    className="license-table-paginationBtn"
                    pageSize={10}
                    onChange={updatePage}
                    current={currentPage}
                    total={pCount}
                />
            </Card>
        </Container>
    );
}

export default LicensesTableList;
