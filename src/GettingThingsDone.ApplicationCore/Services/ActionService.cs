﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GettingThingsDone.ApplicationCore.Helpers;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;
using static GettingThingsDone.Contracts.Interface.ServiceResult;

namespace GettingThingsDone.ApplicationCore.Services
{
    // This is a sample implementation.
    // As such, it contains detailed comments to emphases the general principle.
    // These kind of comments shouldn't be in the real-life code.
    public class ActionService : IActionService
    {
        private readonly IAsyncRepository<Action> _actionRepository;
        private readonly IRepository<ActionList> _listRepository;
        private readonly IRepository<Project> _projectRepository;

        // Inject repositories and/or all the other needed services.
        public ActionService(
            IAsyncRepository<Action> actionRepository,
            IRepository<ActionList> listRepository,
            IRepository<Project> projectRepository)
        {
            _actionRepository = actionRepository;
            _listRepository = listRepository;
            _projectRepository = projectRepository;
        }

        public async Task<ServiceResult<ActionDto>> GetAction(int id)
        {
            // Get the entity from the repository.
            var action = await _actionRepository.GetById(id);

            // Return the appropriate service result if the
            // requested entity does not exist.
            if (action == null)
                return EntityNotFound<ActionDto>();

            // Translate it into DTO and return.
            return action
                .TranslateTo<ActionDto>()
                .ToOkServiceResult();
        }

        public async Task<ServiceResult<List<ActionDto>>> GetAll()
        {
            return (await _actionRepository
                .GetAll(TrackingOption.WithoutTracking)) // Get the entities from the repository.
                .Select(action => action.TranslateTo<ActionDto>()) // Translate them into DTOs.
                .ToList()
                .ToOkServiceResult(); // And return.
        }

        // This is the simplest implementation usual in CRUD scenarios
        // or CRUD scenarios with additional validation.
        // In complex scenarios creating a new entity and editing existing
        // once can differ significantly. In that case we recommend to
        // split this method into two or more.
        public async Task<ServiceResult<ActionDto>> CreateOrUpdate(ActionDto actionDto)
        {
            // Either create a new entity based on the DTO
            // or change an existing one based on the DTO.
            Action action = actionDto.RepresentsNewEntity
                ? actionDto.TranslateTo<Action>()
                : (await _actionRepository.GetById(actionDto.Id)).CopyPropertiesFrom(actionDto);

            // Check if the entity exists (if it was an update).
            if (action == null)
                return EntityNotFound(actionDto);

            // TODO: Later on we will do the checks here.
            //       So far we assume everything always works fine.

            // Save changes.
            action = await _actionRepository.AddOrUpdate(action);

            // If the DTO was representing a new DTO
            // we need to set the assigned Id.
            // If it already had the Id, it is the same one.
            // So we can simply have an assignment here.

            actionDto.Id = action.Id;

            return Ok(actionDto);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var action = await _actionRepository.GetById(id);

            if (action == null)
                return EntityNotFound();

            await _actionRepository.Delete(action);

            return Ok();
        }

        public async Task<ServiceResult> MoveToList(int id, int listId)
        {
            var action = await _actionRepository.GetById(id);
            if (action == null)
                return EntityNotFound("Action not found.");

            var list = _listRepository.GetById(listId); // TODO: Turn to await once the ListRepository supports it.
            if (list == null)
                return EntityNotFound("List not found.");

            action.List = list;

            await _actionRepository.AddOrUpdate(action);

            return Ok();
        }

        public async Task<ServiceResult> AssignToProject(int id, int projectId)
        {
            var action = await _actionRepository.GetById(id);
            if (action == null)
                return EntityNotFound("Action not found.");

            var project = _projectRepository.GetById(projectId);// TODO: Turn to await once the ProjectRepository supports it.
            if (project == null)
                return EntityNotFound("Project not found.");

            action.Project = project;

            await _actionRepository.AddOrUpdate(action);

            return Ok();
        }
    }
}