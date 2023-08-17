﻿using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Controllers;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Tests.Controller;

public class ClubControllerTests
{
    private ClubController _clubController;
    private IClubRepository _clubRepository;
    private IPhotoService _photoService;
    private IHttpContextAccessor _httpContextAccessor;
    public ClubControllerTests()
    {
        //Dependencies
        _clubRepository = A.Fake<IClubRepository>();
        _photoService = A.Fake<IPhotoService>();
        _httpContextAccessor = A.Fake<HttpContextAccessor>();

        //SUT
        _clubController = new ClubController(_clubRepository, _photoService, _httpContextAccessor);
    }

    [Fact]
    public void ClubController_Index_ReturnsSuccess()
    {
        //Arrange - What do i need to bring in?
        var clubs = A.Fake<IEnumerable<Club>>();
        A.CallTo(() => _clubRepository.GetAll()).Returns(clubs);
        //Act
        var result = _clubController.Index();
        //Assert - Object check actions
        result.Should().BeOfType<Task<IActionResult>>();

    }
    [Fact]
    public void ClubController_Detail_ReturnsSuccess()
    {
        //Arrange - What do i need to bring in?
        var id = 1;
        var club = A.Fake<Club>();
        A.CallTo(() => _clubRepository.GetByIdAsync(id)).Returns(club);
        //Act
        var result = _clubController.Detail(id);
        //Assert - Object check actions
        result.Should().BeOfType<Task<IActionResult>>(); 
    }
}