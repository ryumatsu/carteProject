import React, { useEffect, useState } from 'react';
import debug from 'sabio-debug';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { getTypes } from '../../services/lookUpService';
import PropTypes from 'prop-types';
import licenseServices from '../../services/licenseServices';
import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';
import toastr from 'toastr';
import licenseFormSchema from '../../schemas/licenseFormSchema';
import './license.css';
// import Forms from 'react-bootstrap/Form';
//import Row from 'react-bootstrap/Row';
//import Col from 'react-bootstrap/Col';
import { propTypes } from 'react-bootstrap/esm/Image';

import FileUploaderContainer from '.././FileUploaderContainer';

const _logger = debug.extend('LicensesForm');

function AddLicensesForm() {
    const [stateTypes, setStateTypes] = useState([]);
    const [licenseTypes, setLicenseTypes] = useState([]);
    const state = {
        formData: {
            licenseStateId: '',
            licenseType: '',
            licenseNumber: '',
            dateExpires: '',
            isVerified: '',
        },
    };

    _logger(state);
    const mapLicenseStateId = () => {
        return stateTypes?.map((licenseStateId, index) => {
            return (
                <option value={licenseStateId?.id} key={`licenseStateId_${licenseStateId?.id}_${index}`}>
                    {licenseStateId.name}
                </option>
            );
        });
    };

    stateTypes.mappingStates = mapLicenseStateId();

    const mapLicenseTypeId = () => {
        return licenseTypes.map((licenseType, index) => {
            return (
                <option value={licenseType.id} key={`licenseType${licenseType.id}_${index}`}>
                    {licenseType.name}
                </option>
            );
        });
    };

    licenseTypes.mappingLicenseTypes = mapLicenseTypeId();

    const clickHandler = (values) => {
        _logger('values', values.licenseStateId);
        values.licenseStateId = parseInt(values.licenseStateId);
        values.licenseType = parseInt(values.licenseType);
        if (values.isVerified === '0') {
            values.isVerified = false;
        } else {
            values.isVerified = true;
        }
        licenseServices.addLicense(values).then(onHandleSubmitSuccess).catch(onHandleSubmitError);
    };

    const onHandleSubmitSuccess = (response) => {
        _logger('onHandleSubmit Success toastr', response);
        toastr.success('License Submitted successfully', 'Good Job!');
    };

    const onHandleSubmitError = (err) => {
        _logger('onHandleError toastr', err);
        toastr.error('Something went wrong!', 'License was not added successfully');
    };

    useEffect(() => {
        _logger('useEffect fx to call list of states for drop down menu');
        getTypes(['States', 'LicenseTypes']).then(onGetStateSuccess).catch(onGetStateError);
    }, []);

    const onGetStateSuccess = (response) => {
        const states = response.item.states;
        const licenseTypes = response.item.licenseTypes;
        _logger('onGetStateSuccess function', states);
        setStateTypes(states);
        setLicenseTypes(licenseTypes);
    };

    const onGetStateError = (err) => {
        _logger(err, 'GetState error');
        toastr.error('Something went wrong! states were not retrieved.');
    };

    return (
        <React.Fragment>
            <div key={propTypes.Id} className="flexbox-container">
                <Card className="flexbox-item flexbox-item-1">
                    <Card.Title>
                        <h1>Add a License</h1>
                    </Card.Title>
                    <Card.Body>
                        <Formik
                            enableReinitialize={true}
                            initialValues={state.formData}
                            onSubmit={clickHandler}
                            validationSchema={licenseFormSchema}>
                            <Form>
                                <div className="form-group">
                                    <label htmlFor="licenseStateId">License State</label>
                                    <Field
                                        type="number"
                                        component="select"
                                        name="licenseStateId"
                                        className="form-select">
                                        <option value="">Select State</option>
                                        {stateTypes.mappingStates}
                                    </Field>
                                    <ErrorMessage
                                        name="licenseStateId"
                                        component="div"
                                        className="has-error"></ErrorMessage>
                                </div>
                                &nbsp;
                                <div className="form-group">
                                    <label htmlFor="licenseType">License Type</label>
                                    <Field type="number" component="select" name="licenseType" className="form-select">
                                        <option value="">Select License Type</option>
                                        {licenseTypes.mappingLicenseTypes}
                                    </Field>
                                    <ErrorMessage
                                        name="licenseType"
                                        component="div"
                                        className="has-error"></ErrorMessage>
                                </div>
                                &nbsp;
                                <div className="form-group">
                                    <label htmlFor="licenseNumber">License Number</label>
                                    <Field
                                        type="string"
                                        name="licenseNumber"
                                        className="form-control"
                                        placeholder="Enter License Number"
                                    />
                                    <ErrorMessage
                                        name="licenseNumber"
                                        component="div"
                                        className="has-error"></ErrorMessage>
                                </div>
                                &nbsp;
                                <div className="form-group">
                                    <label htmlFor="DateExpires">Date Expires</label>
                                    <Field type="date" name="dateExpires" className="form-control" />
                                    <ErrorMessage
                                        name="dateExpires"
                                        component="div"
                                        className="has-error"></ErrorMessage>
                                </div>
                                &nbsp;
                                <div className="form-group">
                                    <label htmlFor="isVerified">Is the license Verified?</label>
                                    <ErrorMessage
                                        name="isVerified"
                                        component="div"
                                        className="has-error"></ErrorMessage>
                                    <Field type="bool" component="select" className="form-select" name="isVerified">
                                        <option defaultValue>Select Appropriate option</option>
                                        <option value={1}>Yes</option>
                                        <option value={0}>No</option>
                                    </Field>
                                    <Button variant="info" type="submit" className="submit-form-btn-license">
                                        Submit
                                    </Button>
                                </div>
                            </Form>
                        </Formik>
                    </Card.Body>
                </Card>

                <div key={propTypes.Id} className="flexbox-item flexbox-item-2">
                    <Card.Body>
                        <FileUploaderContainer />
                    </Card.Body>
                </div>
            </div>
        </React.Fragment>
    );
}

AddLicensesForm.propTypes = {
    license: PropTypes.shape({
        licenseStateId: PropTypes.number.isRequired,
        licenseType: PropTypes.number.isRequired,
        licenseNumber: PropTypes.string.isRequired,
        dateExpires: PropTypes.string.isRequired,
        isVerified: PropTypes.bool.isRequired,
    }),
};

export default AddLicensesForm;
